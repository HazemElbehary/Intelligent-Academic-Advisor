from flask import Flask, render_template
from recommendation import recommendation_bp
from chatBot import chatbot_bp
from flask_cors import CORS
from chatBot.utils import preload_models
from functools import lru_cache

app = Flask(__name__)
CORS(app)

app.config['SECRET_KEY'] = '66ffb2ab897358aea90e01944324631c'  # Required for session management

# Register the recommendation blueprint
app.register_blueprint(recommendation_bp, url_prefix='/recommendation')
app.register_blueprint(chatbot_bp, url_prefix='/chatbot')

@app.route('/')
def home():
    return render_template('home.html')

# Preload models when the app is created (not just when run directly)
print("🚀 Starting application and preloading models...")
print("⏱️  This may take 30-60 seconds on first run...")
preload_models()
print("✅ All models loaded successfully!")

@lru_cache(maxsize=500)  # Increase cache size
def call_llm_cached(prompt):
    if __name__ == '__main__':
        print("🌐 Starting Flask server...")
        app.run(debug=True)
