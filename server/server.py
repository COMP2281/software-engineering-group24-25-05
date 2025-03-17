from openai import OpenAI
from enum import Enum
from flask import Flask, request

CLIENT = OpenAI()


SYSTEM_COMMAND = """
You are a teaching assistant in an educational game aimed at teaching students
more about computer science. You will be given a question, and you should
output four possible answers to that question. You should also provide an
updated version of the question to ask, making it more suitable for
presentation in an educational video game.

The answers should be concise. As far as possible, answers should be no longer
than two sentences.

The adjusted question should not contain precursors such as "Can you solve this
challenge?", "What about this question?", or "Can you answer this?". You should
only provide the actual question.

The output should be a JSON object.

Here are some examples:

Input: "2 + 2"
Output:
{
    "question": "What is 2 + 2?",
    "answer": {
        "correct": "4",
        "incorrect": [
            "2",
            "10",
            "8"
        ]
    }
}

Input: "What part of the CPU does arithmetic?"
Output:
{
    "question": "Which part of the Central Processing Unit (CPU) is responsile for arithmetic?",
    "answer": {
        "correct": "The Arithmetic and Logic Unit (ALU)",
        "incorrect": [
            "The Memory Bus",
            "The Accumulator",
            "Memory Addresses"
        ]
    }
}

Input: "What does RAM do?"
Output:
{
    "question": "What is the role of RAM (Random Access Memory) in a computer?",
    "answer": {
        "correct": "It temporarily stores data and programs that the CPU is currently using.",
        "incorrect": [
            "It permanently saves files and documents.",
            "It processes graphical content.",
            "It connects the computer to the internet."
        ]
    }
}
"""


class OpenAIModel(Enum):
    O3Mini = "o3-mini"
    GPT45Preview = "gpt-4.5-preview"
    GPT4o = "gpt-4o"
    GPT4oMini = "gpt-4o-mini"
    O1 = "o1"
    O1Mini = "o1-mini"
    ChatGPT4oLatest = "chatgpt-4o-latest"


class OpenAIRole(Enum):
    User = "user"
    System = "system"
    Assistant = "assistant"


class OpenAIRequest:
    def __init__(self, model: OpenAIModel):
        self.model: OpenAIModel = model
        self.messages: list[dict[str, str]] = []

    def append_message(self, role: OpenAIRole, content: str):
        self.messages.append({"role": role.value, "content": content})

    def send(self):
        global CLIENT

        response = CLIENT.responses.create(model=self.model.value, input=self.messages)

        return response.output_text


app = Flask(__name__)


@app.route("/hello")
def hello():
    return "Hello, World"


@app.route("/ask", methods=["GET", "POST"])
def serve_question():
    if request.method == "POST":
        question = request.form.get("question")

        if question is None:
            return "Invalid request", 400

        ai_response = OpenAIRequest(OpenAIModel.GPT4oMini)
        ai_response.append_message(OpenAIRole.System, SYSTEM_COMMAND)
        ai_response.append_message(OpenAIRole.User, question)

        return ai_response.send()

    return "Invalid request", 400
