# Requirements Documentation for Shadow Operative

| Name           | CIS username |
| -------------- | ------------ |
| Ben Magowan    | vcwx72       |
| Danial Maqbool | gkzv63       |
| Will Morgan    | sghp78       |
| Edmond Vajda   | njql73       |
| Sami Lodi      | hqcb32       |
| Heria Chen     | qzxl76       |
| Toby Davis     | cltz62       |

Group Number: 5
Date the document was prepared:
Version: 1.0.1

# 1 - Introduction

## 1.1 - Overview and Justification

The purpose of this project is to develop an engaging educational game, based on the CounterSpy universe, that integrates IBM Skills Build content into its gameplay. The game aims to educate players in areas such as Artificial Intelligence, Cybersecurity, and Data Science, using quizzes and interactive elements that adapt dynamically through an AI-driven difficulty system. The player’s goal is to retrieve stolen plans for a new AI system, progressing by responding to skill-based challenges and engaging in interactive boss battles.

This approach transforms traditional educational content into an immersive, game-based experience that enhances learner engagement and retention. It targets individuals pursuing IBM Skills Build knowledge, with potential applications for educational institutions or self-learners interested in complementing their studies with an engaging, interactive tool.

The project’s client is IBM, represented by Mr. John McManara, who leads IBM UK University Programs and serves as an IBM Master Inventor, a UCL Honorary Professor, and Chair of the Institute of Technology Board. IBM aims to make IBM Skills Build content more accessible and engaging through this game.

This document is organized as follows:

- [1 - Introduction](#1---introduction)
  - [1.1 - Overview and Justification](#11---overview-and-justification)
  - [1.2 - Project Scope](#12---project-scope)
  - [1.3 - System Description](#13---system-description)
- [2 - Solution Requirements](#2---solution-requirements)
  - [2.1 - Requirements Elicitation](#21---requirements-elicitation)
  - [2.2 - Behavioural Requirements](#22---behavioural-requirements)
- [3 - Project Development](#3---project-management)
  - [3.1 - Risks and Issues](#31---risks-and-issues)
  - [3.2 - Development Approach](#32---development-approach)
  - [3.3 - Project Schedule](#33---project-schedule)

## 1.2 - Project Scope

The project addresses the need for an interactive educational tool that merges IBM Skills Build content with engaging gameplay mechanics. By incorporating quizzes linked to IBM Skills Build badges, the game provides a practical, enjoyable way for users to solidify their knowledge in Artificial Intelligence, Cybersecurity, and Data Science.

Project objectives:

- **Improve Engagement**: Use elements like quizzes, progressive difficulty, and AI-driven adaptations to create an enjoyable learning experience.
- **Enable Personalized Learning**: Dynamically adjust question difficulty based on player performance, tailoring the challenge to each user.
- **Support Goal-Oriented Learning**: Reinforce knowledge through boss battles and progressively challenging content that helps learners retain key concepts.

The project’s primary stakeholders include IBM Skills Build content providers, educational institutions, and self-learners seeking an adaptive, game-based learning experience that complements traditional study methods.

## 1.3 - System Description

Shadow Operative is a game structured to integrate IBM Skills Build content into a dynamic learning environment. The game combines quiz mechanics with an adventure format, where players progress by completing challenges that become more difficult based on their past performance. Unique boss battles and in-game challenges serve as checkpoints, testing players’ mastery of IBM Skills Build topics.

### Existing Solutions and Research

While platforms like Duolingo and Kahoot! offer gamified learning, they lack the real-time AI adaptations and direct IBM Skills Build content integration found in Shadow Operative. This project offers AI-driven personalization, which tailors difficulty specifically to enhance engagement and knowledge retention for users in IBM Skills Build areas.

### System Design and Integration

The game’s design will translate IBM Skills Build content into in-game scenarios and questions, with core features including:

- **Question Generation**: IBM Skills Build questions populate different levels and scenarios.
- **Dynamic Difficulty Adjustment**: AI algorithms monitor performance to optimize challenge levels.
- **Boss Interactions**: Gameplay elements, such as boss battles, serve as knowledge assessments that reinforce learning.

Through these features, Shadow Operative innovatively combines IBM Skills Build content with an interactive, adaptive gaming experience for enhanced skill acquisition.

# 2 - Solution Requirements

## 2.1 - Requirements Elicitation

## 2.2 - Behavioural Requirements

# 3 - Project Management

## 3.1 - Risks and Issues

## 3.2 - Development Approach

### 3.2.1 - Incremental Model

* The Incremental Model divides the project into smaller, manageable portions (increments). Each increment represents a subset of the full game functionality.

* With each increment, you develop a part of the game and deliver it. Once that part is functional, you can move on to the next one.

* Each new increment builds upon the previous one, allowing you to add new features, mechanics, or puzzle elements step by step.

### 3.2.2 - In the context of the project

1. Gradual Progress: Since our game includes different mechanics (e.g., running, jumping, sliding), puzzles, and IBM skill badge questions, we could develop one feature at a time. For example, we can first build core mechanics, then puzzles, and then implement the card system for enhancing IBM skills.

2. Feedback after Each Increment: After each partial development done , we can test it and get the feedback. This ensures that individual components work as intended before combining them into the final game.

3. Risk Management: The Incremental Model is useful when different features have varying complexities. It helps manage risk by addressing simpler features first and more complex ones later, ensuring a steady development pace.

4. Flexibility: when we developing the program , client might change the requirement or we need to adjust the direction or the function of our project , while incremental model plays a very important role to help us prevent from deleting a large number of previous documents

5. Player engagement: incremental model means we can republic our game in advance even thought we haven't finished the whole game , we can get the feedback from the client and vary our direction or adjust our code base on the feedback immediately .

6. Suitable for small team : Since we are a small which only has 7 people , the incremental model help us allocate the work more efficacy , we only need to focus on the specific content each time , and makes all the people engaged , also can clearly keep tracking work progress.

7. Simplicity: The incremental model provides straightforward, predictable development process, also offers a structured, step-by-step approach with clear deliverables at each stage.

## 3.3 - Project Schedule

### Art / Vision Design

* due to **Unity** offering an open source template that closely
matches the client's vision of the final game we have decided to
have an extended period of planning the aspects of the game such as

1) game mechanics (stealth, guns, puzzles)
2) art direction

### RAG Analysis and the Questions Bank

* from `2024-11-06` to `2024-11-20` we want to finish the client's request for
a RAG analysis of the IBM courses

* which will be needed in order to create the questions bank for the puzzles
of the game, as per request of the client, the puzzles will be related to the **IBM Skills Build**
courses

### C# / Unity Acquaintance

* not all of us have worked in the past with `C#/Unity`, due to this we have planned
a long lasting period (until the end Christmas Vacation) where we can familiarize ourselves
with the tools at our disposal

### Refactor Unity template

* as mentioned in `Art / Vision Design` **Unity** offers a starter template, which
will be refactored in such a way to suit our needs and / or coding style

### Game Mechanics Implementation and Test Plan

* these phases begin at the same time and we hope to identify a viable
testing plan while developing game mechanics such as:

1) basic movement
2) stealth mechanics
3) combat mechanics
4) enemies

### Level Design and IBM Education Integration

* after the `Game Mechanics Implementation` phase once we have
a solid foundation we can start the `Level Design` phase which
consists in the creation of **3 stages**

* and create puzzles for the 3 stages which will **act as a
progression system**

* we will use the **questions bank** made in the first stages
of the development to enrich the puzzles with IBM Skills Build questions

### Technical Report and Product Presentation

* they will start to run in parallel once we are at the end of the development
cycle with the `Game Mechanics Implementation`

### Gantt Chart

<p align="center">
    <img src="./images/gantt-chart.png">
</p>

### Key deadlines

* `2024-11-28`: Finish client RAG Analysis and Questions Bank
* `2024-03-21`: Finish all game deadlines (mechanics, level design, IBM integration)
