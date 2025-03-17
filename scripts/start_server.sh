#!/bin/bash

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

command_exists() {
    command -v "$1" >/dev/null 2>&1
}

if command_exists python3; then
    echo "Python is installed."
else
    echo "Python is not installed. Please install Python 3."
    exit 1
fi

if command_exists pip3; then
    echo "pip is installed."
else
    echo "pip is not installed. Please install pip."
    exit 1
fi

if python3 -c "import flask" >/dev/null 2>&1; then
    echo "Flask is already installed."
else
    echo "Flask is not installed. Installing Flask..."
    pip3 install flask
fi

if [ -z "$OPENAI_API_KEY" ]; then
    echo "Error: The OPENAI_API_KEY environment variable is not set."
    exit 1
fi

echo "Running the Flask app..."
flask --app $SCRIPT_DIR/../server/server.py run
