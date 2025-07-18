import json
import os
import requests
from langchain.document_loaders import JSONLoader
from langchain.text_splitter import RecursiveCharacterTextSplitter
from langchain.embeddings import HuggingFaceEmbeddings
from langchain.vectorstores import FAISS
from langchain.chains import RetrievalQA
from langchain import HuggingFaceHub
from langchain.schema import Document
from huggingface_hub import InferenceClient
from functools import lru_cache

# Global variables for caching
_embeddings = None
_vector_store = None
_retriever = None

def get_embeddings():
    """Get or create embeddings instance (singleton pattern)"""
    global _embeddings
    if _embeddings is None:
        print("🔄 Loading E5 embeddings model (this may take a moment on first run)...")
        _embeddings = HuggingFaceEmbeddings(model_name="intfloat/e5-large-v2")
        print("✅ E5 embeddings model loaded successfully!")
    return _embeddings

def get_retriever():
    """Get or create retriever instance (singleton pattern)"""
    global _retriever
    if _retriever is None:
        vector_store = create_vector_store("chatBot\data")
        _retriever = vector_store.as_retriever(search_kwargs={"k": 20})
    return _retriever

# 1️⃣ Extract text from JSON recursively
def extract_text_from_json(data, prefix=""):
    text_chunks = []
    if isinstance(data, dict):
        for key, value in data.items():
            new_prefix = f"{prefix} -> {key}" if prefix else key
            text_chunks.extend(extract_text_from_json(value, new_prefix))
    elif isinstance(data, list):
        for idx, item in enumerate(data):
            new_prefix = f"{prefix} [{idx}]"
            text_chunks.extend(extract_text_from_json(item, new_prefix))
    else:
        text_chunks.append(f"{prefix}: {data}")
    return text_chunks

# 2️⃣ Load and flatten all JSON bylaw files
def create_vector_store(folder_path, vectorstore_path="bylaws_vector_index"):
    global _vector_store
    
    # Return cached vector store if available
    if _vector_store is not None:
        return _vector_store
    
    # the FAISS index file that save_local() creates
    index_file = os.path.join(vectorstore_path, "index.faiss")

    # 1) If that file exists, skip everything and just load the store:
    if os.path.exists(index_file):
        print(f"✔ Found existing FAISS index at '{index_file}'. Loading vector store…")
        embeddings = get_embeddings()  # Use cached embeddings
        _vector_store = FAISS.load_local(
            vectorstore_path,
            embeddings,
            allow_dangerous_deserialization=True
        )
        print("✅ Vector store loaded successfully!")
        return _vector_store

    # 2) Otherwise, go ahead and build it from your JSONs
    print("✚ No existing index found—creating new vector store…")

    all_texts = []

    for fn in os.listdir(folder_path):
        if fn.lower().endswith(".json"):
            # filepath = os.path.join(folder_path, filename)
            with open(os.path.join(folder_path,fn), encoding='utf-8') as f:
                data = json.load(f)
            all_texts.extend(extract_text_from_json(data))  # Flatten directly

    text = "\n".join(all_texts)

    # 3️⃣ Split text into chunks for retrieval
    splitter = RecursiveCharacterTextSplitter(chunk_size=1024, chunk_overlap=150) #512  #100
    docs = splitter.create_documents([text])

    # Optional: Add metadata to each document chunk
    for d in docs:
        d.metadata = {"source": "bylaw"}  # Add more specific metadata if needed

    # embed & save
    embeddings = get_embeddings()  # Use cached embeddings
    _vector_store = FAISS.from_documents(docs, embeddings)
    _vector_store.save_local(vectorstore_path)
    print(f"✔ New vector store saved at '{vectorstore_path}'")
    return _vector_store

@lru_cache(maxsize=100)
def call_llm_cached(prompt):
    return call_llm(prompt)

def call_llm(prompt):
    try:
        response = requests.post(
            url="https://openrouter.ai/api/v1/chat/completions",
            headers={
                "Authorization": "Bearer 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",
                "Content-Type": "application/json",
                # "HTTP-Referer": "http://your-site-url.com",  # Optional
                "X-Title": "BylawAssistant",  # Optional
            },
            data=json.dumps({
                "model": "deepseek/deepseek-r1-0528-qwen3-8b:free",  #   qwen/qwen3-30b-a3b:free  #deepseek/deepseek-prover-v2:free
                "messages": [
                    {"role": "system", "content": "You are a helpful assistant trained on university academic bylaws."},
                    {"role": "user", "content": prompt}
                ]
            })
        )
        response.raise_for_status()
        return response.json()['choices'][0]['message']['content']
    except Exception as e:
        return f"⚠️ LLM Error: {str(e)}"

# 5️⃣ Define query handler using retriever and custom LLM
def process_query(user_query):
    """Retrieve context and generate answer using LLM."""
    try:
        # Use cached retriever for faster access
        retriever = get_retriever()
        docs = retriever.get_relevant_documents(user_query)

        if not docs:
            return "⚠️ No relevant documents found."

        # Concatenate retrieved document texts
        context = "\n\n".join(doc.page_content for doc in docs)

        # Prompt LLM with context + question
        prompt = (
            f"You are an assistant helping answer questions based on university bylaws.\n\n"
            f"Context:\n{context}\n\n"
            f"Question: {user_query}\n\n"
            f"Helpful Answer:"
        )

        # Use cached LLM call for repeated queries
        return call_llm_cached(prompt)

    except Exception as e:
        return f"⚠️ Error: {str(e)}"

# Optional: Function to preload everything at startup
def preload_models():
    """Preload embeddings and vector store at application startup"""
    print("🚀 Preloading models for faster response...")
    
    print("📥 Step 1/3: Loading E5 embeddings model...")
    folder_path = "chatBot\data"
    
    print("📥 Step 2/3: Loading vector store...")
    create_vector_store(folder_path)
    
    print("📥 Step 3/3: Initializing retriever...")
    get_retriever()  # Preload retriever as well
    
    print("✅ All models preloaded successfully!")
    print("🎯 Ready to answer questions instantly!")