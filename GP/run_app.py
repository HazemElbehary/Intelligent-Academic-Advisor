#!/usr/bin/env python3
"""
Simple startup script for the Academic Advisor System
"""
import os
import sys
import time

def main():
    print("🎓 Academic Advisor System")
    print("=" * 40)
    print()
    
    # Check if required files exist
    required_files = [
        "app.py",
        "chatBot/",
        "recommendation/",
        "templates/",
        "static/"
    ]
    
    print("📋 Checking system files...")
    for file_path in required_files:
        if os.path.exists(file_path):
            print(f"✅ {file_path}")
        else:
            print(f"❌ {file_path} - Missing!")
            return False
    
    print()
    print("🚀 Starting the application...")
    print("⏱️  Models will load automatically during startup")
    print("✅ Once loaded, all questions will be fast!")
    print()
    print("🌐 Server will start at: http://localhost:5000")
    print("📱 Chatbot available at: http://localhost:5000/chatbot/chatbot")
    print()
    print("Press Ctrl+C to stop the server")
    print("=" * 40)
    
    # Import and run the Flask app
    try:
        from app import app
        app.run(debug=True, host='0.0.0.0', port=5000)
    except KeyboardInterrupt:
        print("\n👋 Server stopped by user")
    except Exception as e:
        print(f"\n❌ Error starting server: {e}")
        return False
    
    return True

if __name__ == "__main__":
    success = main()
    if not success:
        sys.exit(1)