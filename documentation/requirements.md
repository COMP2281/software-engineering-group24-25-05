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

The purpose of this project is to develop an engaging educational game, based on the CounterSpy universe, that integrates IBM Skills Build content into its gameplay.

This approach transforms traditional educational content into an immersive, game-based experience that enhances learner engagement and retention. It targets individuals pursuing IBM Skills Build knowledge, with potential applications for educational institutions or self-learners interested in complementing their studies with an engaging, interactive tool.

The project’s client is IBM, represented by Mr. John McManara, who leads IBM UK University Programs and serves as an IBM Master Inventor, a UCL Honorary Professor, and Chair of the Institute of Technology Board. IBM aims to make IBM Skills Build content more accessible and engaging through this game.

This document is organised as follows:

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

The game aims to educate players in areas such as Artificial Intelligence, Cybersecurity, and Data Science, using quizzes and interactive elements that adapt dynamically through an AI-driven difficulty system. The player’s goal is to retrieve stolen plans for a new AI system, progressing by responding to skill-based challenges and engaging in interactive boss battles.

Project objectives:

- **Improve Engagement**: Use elements like quizzes, progressive difficulty, and AI-driven adaptations to create an enjoyable learning experience.
- **Enable Personalised Learning**: Dynamically adjust question difficulty based on player performance, tailoring the challenge to each user.
- **Support Goal-Oriented Learning**: Reinforce knowledge through boss battles and progressively challenging content that helps learners retain key concepts.

The project’s primary stakeholders include IBM Skills Build content providers, educational institutions, and self-learners seeking an adaptive, game-based learning experience that complements traditional study methods.

## 1.3 - System Description

Shadow Operative is a game structured to integrate IBM Skills Build content into a dynamic learning environment. The game combines quiz mechanics with an adventure format, where players progress by completing challenges that become more difficult based on their past performance. Unique boss battles and in-game challenges serve as checkpoints, testing players’ mastery of IBM Skills Build topics.

### Existing Solutions and Research

While platforms like Duolingo and Kahoot! offer gamified learning, they lack the real-time AI adaptations and direct IBM Skills Build content integration found in Shadow Operative. This project offers AI-driven personalisation, which tailors difficulty specifically to enhance engagement and knowledge retention for users in IBM Skills Build areas.

### System Design and Integration

The game’s design will translate IBM Skills Build content into in-game scenarios and questions, with core features including:

- **Question Generation**: IBM Skills Build questions populate different levels and scenarios.
- **Dynamic Difficulty Adjustment**: AI algorithms monitor performance to optimise challenge levels.
- **Boss Interactions**: Gameplay elements, such as boss battles, serve as knowledge assessments that reinforce learning.

Through these features, Shadow Operative innovatively combines IBM Skills Build content with an interactive, adaptive gaming experience for enhanced skill acquisition.

# 2 - Solution Requirements

## 2.1 - Requirements Elicitation

Report on the steps undertaken by the group to elicit the client’s requirements, to develop them into User Stories, and to refine them into behavioural specifications using the Gherkin language. You should convince the reader that the specifications you have arrived at represent the requirements of the client to the best of your ability. Report on any difficulties or challenges you encountered and how you overcame them (if relevant). This section may mention any meetings or correspondence you had with the client, although screenshots of emails or DMs are not appropriate here. You can also report on the outcomes of any internal meetings that were especially influential on the refinement and validation of the requirements and specify any established methods or approaches you used to achieve this. Recommended 1 page

- Describes activities taken place during the project intended to elicit requirements
  Clearly shows how efforts to understand the client’s requirements influenced the design of the specifications
- Describes the process of gathering and validating User Stories, citing any relevant approaches or methodologies
  Describes the process by which User Stories informed the preparation of Gherkin features, citing any relevant approaches or methodologies
  Accounts for any difficulties encountered in preparing the User Stories or the Gherkin pseudocode listing and how they were overcome

### 2.1.1. **Eliciting Requirements:**

To gain a thorough understanding of the client's needs for the CounterSpy educational game, the team reached out via email on 15/10/24, aiming to address specific concerns about feature integration, gameplay mechanics, and AI-driven educational content. The client responded on 17/10/24, clarifying the project's focus on seamlessly incorporating IBM Skills Build content (including AI, Cybersecurity, and Data Analytics) into the gameplay. This exchange allowed us to refine the project scope, confirm the inclusion of adaptive learning through AI algorithms, and remove features that were identified as redundant. These insights also guided us in structuring our team roles based on each members strengths and development areas.

Following this, we held an internal meeting to discuss our objectives in depth and to identify how each member's skills could contribute to creating a compelling, interactive experience that aligns with CounterSpy's Cold War-inspired, side-scrolling stealth gameplay. This meeting helped us generate additional questions for the client regarding specific elements, such as how best to implement game mechanics that motivate players to learn and progress through adaptive, question-based challenges. We also explored potential design inspirations, honing in on critical aspects like gameplay mechanics, the Cold War-era visual style, and strategies to maintain a consistent thematic experience that embodies the CounterSpy universe. Within this meeting we all came to a consesus that the card feature of the game did not fit our game's brief, which we addressed in a follow up email, in which the client agreed, and decided to remove that feature from the brief.

**ADD MORE AFTER MEETING - Mention videos sent**

Within our first meeting, we address some key concerns and expressed our moscow with our client, seeing if he agrees with our priorties and if he believes a feature is more important. __________ etcetcetc

### Developing User Stories

* *Example Story* : "As a player, I want to answer progressively challenging AI-related questions to advance in levels, so that my learning adapts to my gameplay."

HOW GHERKIN SHOULD LOOK
Scenario: Progression through levels by answering AI questions
Given a player completes a level with a correct answer
When they answer a cybersecurity question correctly
Then they advance to the part of the level/next level



add diagram? Use with MOSCOW

#### Challenges and Solutions

* **Challenge** : Balancing gameplay with educational content.
* **Solution** : Incorporating adaptive AI that adjusts question difficulty to player performance.
* **Challenge** : Defining behavioral requirements that cater to both learning and entertainment.
* **Solution** : Scenario-driven requirements helped visualize player journeys, ensuring learning moments felt natural within the game.


### 2.1.4. **Finalizing Specifications:**

* Conclude by reinforcing that the final User Stories and Gherkin features represent the best possible interpretation of the client’s requirements.
* Briefly mention any final client feedback or sign-offs that confirmed alignment with their expectations.

Each section should connect the methods and challenges directly back to the project outcomes, showing how they informed the specifications and proving that they are tailored to the client’s needs.

## 2.2 - Behavioural Requirements

# 3 - Project Management

## 3.1 - Risks and Issues

## 3.2 - Development Approach

## 3.3 - Project Schedule
