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

* Group Number: 5
* Date the document was prepared: `11/20/2024`
* Version: 1.0.2

# 1 - Introduction

## 1.1 - Overview and Justification

The purpose of this project is to develop an engaging educational game, based on the CounterSpy universe, that integrates IBM Skills Build content into its gameplay. This approach transforms their current educational material into an immersive, game-based experience, enhancing both learner engagement and knowledge retention; In doing so, the project addresses the challenge of making educational content more appealing and interactive, something that IBM have identified as a key area of improvement.

The project benefits individuals who currently learn via the IBM Skills Build, including self-learners and educational institutions seeking interactive tools to complement their curricula. By gamifying the learning process, this solution has the potential to expand the reach and effectiveness of IBM Skills Build programs, particularly for younger learners or those in need of alternative learning methods.

The project’s client is IBM, represented by John McManara, who leads IBM UK University Programs and serves as an IBM Master Inventor. IBM’s overarching goal is to increase the engagement of Skills Build content. This project aligns with IBM’s strategic aim to integrate cutting-edge solutions into its educational offerings.

This document is organised as follows:

- [1 - Introduction](#1---introduction)
  - [1.1 - Overview and Justification](#11---overview-and-justification)
  - [1.2 - Project Scope](#12---project-scope)
  - [1.3 - System Description](#13---system-description)
    - [1.3.1 - System Overview](#131-system-overview)
    - [1.3.2 - Research into Alternative Solutions](#132-research-into-alternative-solutions)
    - [1.3.3 - Proposed Solution's Technical Features](#133-proposed-solutions-technical-features)
    - [1.3.4 - Integration Considerations](#134-integration-considerations)
- [2 - Solution Requirements](#2---solution-requirements)
  - [2.1 - Requirements Elicitation](#21---requirements-elicitation)
    - [2.1.1 - Challenges Encountered](#211-challenges-encountered)
    - [2.1.2 - Requirements Validation](#211-requirements-validation)
  - [2.2 - Behavioural Requirements](#22---behavioural-requirements)
    - [2.2.1 - Stealth Gameplay](#221-stealth-gameplay)
    - [2.2.2 - Educational Engagement](#222-educational-engagement)
    - [2.2.3 - Pause Screen](#223-pause-screen)
    - [2.2.4 - AI Boss](#224-ai-boss)
    - [2.2.5 - SFX](#225-sfx)
    - [2.2.6 - Stylised Cold War era graphics](#226-stylised-cold-war-era-graphics)
    - [2.2.7 - Mission System](#227-mission-system)
    - [2.2.8 - Movement Mechanics](#228-movement-mechanics)
    - [2.2.9 - Combat Mechanics](#229-combat-mechanics)
- [3 - Project Development](#3---project-management)
  - [3.1 - Risks and Issues](#31---risks-and-issues)
    - [3.1.1 - Group Risks](#311-group-risks)
    - [3.1.2 - Client Risks](#312-client-risks)
    - [3.1.3 - Software Development Methodology Risks](#313-software-development-methodology-risks)
    - [3.1.4 - AI Risks](#314-ai-risks)
  - [3.2 - Development Approach](#32---development-approach)
    - [3.2.1 - Incremental Model](#321-incremental-model)
    - [3.2.2 - In the contect of the project](#322-in-the-context-of-the-project)
  - [3.3 - Project Schedule](#33---project-schedule)
    - [3.3.1 - Gantt Chart](#331-gantt-chart)
    - [3.3.2 - Art / Vision Design](#332-art--vision-design)
    - [3.3.3 - RAG Analysis and the Question Bank](#333-rag-analysis-and-the-questions-bank)
    - [3.3.4 - C# / Unity Acquaintance](#334-c--unity-acquaintance)
    - [3.3.5 - Game Mechanic Implementation and Test Plan](#335-game-mechanics-implementation-and-test-plan)
    - [3.3.6 - Level Design and IBM Education Integration](#336-level-design-and-ibm-education-integration)
    - [3.3.7 - Technical Report and Product Presentation](#337-technical-report-and-product-presentation)
    - [3.3.8 - Key Deadlines](#338-key-deadlines)

## 1.2 - Project Scope

### 1.2.1 Purpose and Overall Goals

The primary goal of the Shadow Operative project is to enhance learner engagement and retention by transforming IBM Skills Build concepts into an immersive, interactive gaming experience. By integrating educational content within gameplay, the project seeks to address the limitations of traditional learning methods, such as low engagement and lack of interactivity, with a more innovative and appealing approach.

Through the combination of stealth-based challenges and gamified learning mechanics, the project aims to not only make learning enjoyable but also improve understanding and application of key concepts. The game aspires to serve as a scalable, versatile educational tool that can support a wide range of learners and promote the adoption of IBM Skills Build across diverse educational environments.

### 1.2.2 Stakeholders

- **Primary Stakeholder:**
  IBM, represented by John McManara, who leads IBM UK University Programs and serves as an IBM Master Inventor. IBM's aim is to improve accessibility to their Skills Build content and enhance learner outcomes through innovative tools.

- **Target Users:**

  - Students in educational institutions who wish to complement their studies with engaging, interactive tools.
  - Educators looking for gamified solutions to teach IBM Skills Build concepts in a classroom setting.
  - Self-learners pursuing certifications to improve their skills in fields such as cybersecurity, AI, and data analytics.

- **Secondary Stakeholders:**
  - IBM’s educational partners, who may use this tool to promote IBM Skills Build.
  - Developers and teams within IBM involved in creating or supporting educational technologies.

### 1.2.3 Boundaries

- **Educational Content:**
  The game focuses solely on IBM Skills Build content, avoiding unrelated gameplay elements like heavy combat mechanics. Content is designed to be both engaging and educational, blending narrative with skill-based challenges directly linked to IBM Skills Build objectives.

- **Platform Compatibility:**
  Development is restricted to the Windows platform, prioritising stability and simplicity. Expanding to other platforms, such as macOS, mobile devices, or gaming consoles, is beyond the scope of this project due to resource and time constraints.

- **Multiplayer:**
  The game will only support single-player gameplay to maintain consistency in educational content delivery and simplicity in development. Multiplayer features, such as cooperative or competitive modes, are excluded from the current scope.

- **Graphical and Animation Complexity:**
  To focus on functionality and thematic coherence, the game will avoid advanced graphical techniques, such as photorealistic rendering or motion capture. Instead, it will use stylised visuals that align with its educational goals.

### 1.2.4 Vision

Shadow Operative aims to blend stealth gameplay mechanics with dynamic, question-based educational challenges, creating a unique tool that enhances learning through immersive storytelling. The game will place players in a Cold War-era setting, engaging them in scenarios where success is tied to their mastery of IBM Skills Build concepts.

By leveraging adaptive difficulty, engaging narratives, and a stealth-driven environment, Shadow Operative seeks to inspire the development of new educational tools that merge fun and functionality. The vision is to redefine how learners interact with educational content, shifting from passive consumption to active, enjoyable engagement.

## 1.3 - System Description

### 1.3.1 System Overview

Shadow Operative is an educational game designed to help learners engage with IBM Skills Build content in an immersive and interactive environment. The game places players in the role of a spy during the Cold War, tasked with retrieving stolen plans for a cutting-edge AI system. Through skill-based challenges and AI-driven adaptive difficulty, players are guided through progressively complex levels that reinforce core IBM Skills Build concepts in a gamified format.

### 1.3.2 Research into Alternative Solutions

In designing Shadow Operative, the team conducted extensive research into comparable games and educational platforms to inform and enhance its structure, mechanics, and learning approach. The following games served as key references, with their unique aspects and usefulness to Shadow Operative outlined below:

- **CounterSpy**: A Cold War-era side-scrolling stealth game, CounterSpy serves as a primary thematic reference for Shadow Operative, offering insight into effective use of stealth elements, setting, and art direction within a Cold War context.
  - Limitation: CounterSpy does not integrate educational content, which limits its application in learning environments. Shadow Operative addresses this by embedding educational challenges into the gameplay.
- **This War of Mine**: This side-scrolling survival game provides a perspective on civilian experiences of war, integrating stealth mechanics and adding depth to our Cold War setting by informing character interactions and emotional undertones.
  - Limitation: The game's heavy emotional themes may not be suitable or relevant for younger learners. Shadow Operative maintains engagement while ensuring content is appropriate for educational purposes.
- **Dishonored 2**: Dishonored 2 inspired our enemy awareness systems, and varied approaches to evasion and takedowns. These features are crucial for building the interactive stealth elements in Shadow Operative.
  - Limitation: The complexity of Dishonored 2's mechanics could overwhelm learners. Our game simplifies these systems to ensure accessibility while maintaining engaging gameplay.
- **Batman: Arkham Asylum**: Recognized for its claustrophobic environments and atmospheric design, Batman: Arkham Asylum offers a model for crafting immersive environments and leveraging tight spaces to build tension.
  - Limitation: The combat focus of Batman: Arkham Asylum is less relevant for our stealth-driven gameplay. Shadow Operative focuses purely on stealth to maintain thematic and educational alignment.
- **Kahoot!**: This gamified quiz platform highlights the power of interactive question-based challenges and adaptive difficulty. It directly influences Shadow Operative's question generation system and difficulty adjustments, with timers and music used to create urgency in an educational context.
  - Limitation: Kahoot!'s approach lacks the narrative and immersive elements we aim to include, which Shadow Operative addresses by integrating learning into a cohesive story-driven experience.

### 1.3.3 Proposed Solution’s Technical Features

Shadow Operative integrates the following key technical features to create a unique, adaptive learning experience:

- **Question Generation**: Relevant IBM Skills Build content will populate various levels, embedding knowledge assessments seamlessly into gameplay scenarios. Questions will adapt in difficulty and context to align with players' progress, enhancing engagement and reinforcing learning objectives.
- **Dynamic Difficulty Adjustment**: AI algorithms will monitor and assess player performance, adjusting challenge levels in real-time. This ensures players are consistently engaged and challenged without being overwhelmed, fostering a tailored learning experience.
- **Boss Interactions**: Key encounters, such as boss battles, act as knowledge assessments, where players demonstrate their understanding of core Skills Build concepts in a high-stakes, interactive format that reinforces learning.
- **Stealth Mechanics**: Inspired by CounterSpy and Dishonored 2, Shadow Operative incorporates stealth elements like sneaking, cover mechanics, and enemy awareness to create a tense, immersive experience that complements the game's educational content, keeping the user engaged.

### 1.3.4 Integration Considerations

Shadow Operative is designed as a standalone educational game; however, it is making use of IBM’s AI technology, Granite, allowing the user to make the experience adpative to them, focusing more on their usecase of the system.

# 2 - Solution Requirements

## 2.1 - Requirements Elicitation

To gain a thorough understanding of the client’s needs, our team reached out via email on 15/10/24, aiming to address specific concerns about feature integration, gameplay mechanics, and AI-driven educational content. The client responded on `17/10/24`, clarifying the project’s focus on seamlessly incorporating IBM Skills Build content (including AI, Cybersecurity, and Data Analytics) into the gameplay. This exchange allowed us to refine the project scope, confirm the inclusion of adaptive learning through AI algorithms, and remove features identified as redundant. These insights also guided us in structuring team roles based on each member’s strengths and development areas.

### 2.1.1 Challenges Encountered

Our initial difficulty was organizing our first meeting with the client due to conflicting schedules among team members. This caused a delay in setting up a direct conversation, which impacted our early progress. Additionally, one of our team members was unavailable for a few weeks due to being away, which further complicated scheduling and resource allocation. Despite these obstacles, the team coordinated through asynchronous communication and adjusted roles temporarily to maintain momentum.

Following this, we held an internal meeting to discuss our objectives in depth and to identify how each member’s skills could contribute to creating a compelling, interactive experience that aligns with CounterSpy’s Cold War-inspired, side-scrolling stealth gameplay. This meeting helped us generate additional questions for the client regarding elements such as how best to implement game mechanics that motivate players to learn and progress through adaptive, question-based challenges. We also explored design inspirations, focusing on essential aspects like gameplay mechanics, Cold War-era visual style, and strategies to maintain a consistent thematic experience. During this meeting, we reached a consensus that the card feature did not align with the game’s objectives, which we then proposed for removal in a follow-up email. The client agreed with this decision.

In our first client meeting on 06/11/24, we discussed key project concerns and shared a preliminary MoSCoW prioritization to confirm alignment with the client’s goals. The client agreed that the educational component should be the top priority, as the primary aim is to promote IBM Skills Build badges in a more engaging way than current text-based modules. To address this, we collaboratively designed a core game mechanic: when a player is spotted by a robotic guard, they must answer a multiple-choice question to proceed.

To refine our requirements into behavioral specifications, we used Gherkin language. For example, we created scenarios detailing how the game would present challenges, adjust difficulty dynamically, and integrate IBM Skills Build content seamlessly. This method allowed us to concisely capture the desired system behavior and align it with the client’s educational objectives. This allowed us to create user stories that detailed the player’s journey through the game, from initial engagement to mastery of key concepts, ensuring that our requirements were user-focused and aligned with the client’s vision.

### 2.1.2 Requirement Validation

To ensure that our refined requirements aligned accurately with the client’s needs, we sent a confirmation email summarizing key decisions and requested feedback. The client’s positive response affirmed our direction. We also planned a follow-up meeting to further validate our approach and address any remaining questions, ensuring continuous alignment with the client’s educational goals.

Our team then met to discuss the client’s feedback and refine our understanding of the project’s requirements. We identified key features, such as question generation, dynamic difficulty adjustment, and boss interactions, that would enhance the game’s educational value and align with the client’s objectives.

## 2.2 - Behavioural Requirements

### 2.2.1 Stealth Gameplay

#### User Story

As a player, I want a side-scrolling experience where I can use stealth mechanics to infiltrate enemy bases, so I can emulate spy tactics.

#### Feature

* Stealth Mechanics

#### BR 1.1 Scenario: using cover to avoid detection

- Given the player is near enemy guards
- And the player is within the guard’s line of sight
- When the player moves into cover
- Then the guards should not detect the player

#### BR 1.2 Scenario: making noise near guards

- Given the player is making noise
- When a guard is within earshot
- Then then an investigation state should be triggered
- And the guard should investigate the noise made
- And if the guard detects the player, an alert state is triggered

#### BR 1.3 Scenario: crouch walking to avoid detection

- Given the player is outside any cover
- And the player is within the  guard’s line of sight
- When the player moves slowly in a crouched or stealthy stance
- Then the guards should have a reduced chance of detecting the player
- And the player can bypass guards without triggering an alert if they remain at a safe distance

#### BR 1.4 Scenario: triggering an alert when spotted

- Given the player is within a visible range of the guard’s line of sight
- And the player is not in cover
- When the guard spots the player
- Then an alert state should be triggered
- And nearby guards should move toward the player’s last known position

#### BR 1.5 Scenario: returning to patrol after losing sight of player

- Given guards are in an alert state after spotting the player
- And the player has moved out of their line of sight
- When the guards do not detect the player for a set amount of time
- Then the guards should return to their normal patrol behaviour
- And the alert state should end

#### Rationale

Implementing this feature would contribute significantly to the client’s objectives as the core gameplay feature in the game design desired by the client was Stealth.
Due to the theme of the game being Cold war espionage, stealth is one of the core requirements of the game outlined by the client, therefore implementing this feature would contribute significantly to the clients objective, therefore this feature’s MosCoW prioritisation is “Must Have."

#### MoSCoW Priority: MUST HAVE

This Feature can be considered a “Must have” and takes higher priority than other gameplay mechanics besides Movement.

### 2.2.2 Educational Engagement

#### User Story

As a player, I want to answer AI, cybersecurity, and data analytics questions from the IBM Skills question bank, so that I can test my knowledge and understand IBM Skills build concepts.

#### Feature

* IBM Skills question bank

#### BR 2.1 Scenario: player steals classified document

- Given the player has found a classified document
- When the player steals the document
- Then the player must answer a question related to AI, data analytics, or cybersecurity
- And if the player answers the question correctly, they get 10 coins
- But if the player answers the question incorrectly, guards nearby are triggered into an alert state

#### BR 2.2 Scenario: quiz at the end of the level

- Given the player has reached the final challenge of the level
- When the quiz prompt appears on the screen
- Then the player must answer a series of questions related to AI, data analytics, and cybersecurity
- And if the player answers the questions correctly an accuracy of at least 80%, they can progress to the next level
- But if the player answers incorrectly, they must redo the quiz

#### Rationale

Implementing this feature is would contribute significantly, because client aims to expand beyond their pre-existing website by exploring new mediums to attract more interest in IBM Skills Build.
This feature would ensure that the game’s design aligns with the client’s educational goals, seamlessly integrating the learning aspect into the gameplay, creating a fun and engaging platform to develop IBM Skills Build. As such, this feature has been assigned a MoSCoW prioritisation of “Must Have.”

#### MoSCoW Priority: MUST HAVE

This Feature can be considered a “Must have” and takes higher priority than other gameplay mechanics besides Movement.

### 2.2.3 Pause Screen

#### User Story

As a player, I want to be able to pause my game so that I can take a break from the game, change the settings or exit the game.

#### Feature

* Pause Screen

#### Background

- Given the player is in a level
- When the player presses the designated pause key on the keyboard
- Then the game pauses
- And the Pause screen is displayed
- And buttons are displayed on the screen

#### BR 3.1 Scenario: taking a break from the game

- Given the game is paused
- When the player clicks the resume button
- Then the game resumes
- And the player can pick up where they left off

#### BR 3.2 Scenario: accessing game settings

- Given the game is paused
- When the player selects the settings option from the Pause screen
- Then the settings menu is displayed
- And the player can adjust settings to their preference

#### Rationale

Implementing this feature would enhance the user’s gameplay experience and improve the overall quality of life within the game. Rather than having to complete the game in a single session, players would be able to pause the game, and complete it a later time. Although a pause screen is not explicitly mentioned as a core requirement, this feature adds flexibility and convenience to the game, therefore significantly improving the game’s quality of life, achieving the client’s objectives of a high quality user experience ; Due to the reasons outlined above, this feature’s MoSCoW prioritisation is classified as “Could Have.”

#### MoSCoW Priority: COULD HAVE

This feature is considered a “Could Have” as it is not a mandatory requirement but offers a meaningful improvement to the player experience.

### 2.2.4 AI Boss

#### User Story

As a player, I want an AI boss that challenges the skills I learnt throughout the game, so that I can demonstrate my abilities in stealth, quizzes and combat mechanics under pressure.

#### Feature

* Adaptive Learning

#### BR 4.1 Scenario: testing stealth skills

- Given the player has demonstrated weakness in Stealth based tasks in the game
- When they encounter the final boss
- Then the boss will adapt the the level environment to include stealth based mechanics
- But the main objective is still the final quiz

#### BR 4.2 Scenario: quiz adjustment

- Given the player has previously struggled with certain topics
- When they encounter the final boss
- Then the boss can adapt to the player’s weaknesses
- And generate a custom quiz for the player
- And include the player’s weaknesses in the custom quiz

#### BR 4.3 Scenario: combat mechanics adjustment

- Given the player has demonstrated weakness in combat abilities
- When the player encounters the final boss
- Then can adjust the level to include to include an element of combat based mechanics alongside the final quiz
- But the main objective is still the final quiz

#### Rationale

Implementing this feature would significantly contribute to the client’s overall goals and objectives, as AI algorithms were explicitly specified in the Technical Requirements of the Product/Service Requirements.
The use of AI to personalise the learning experience and adapt the game to the player’s performance is a key directive from the client. By incorporating this feature, the game ensures alignment with the client’s vision and technical specifications. Whilst also additionally building upon the education aspect of the game, enabling the user to be tested and learn based on what it may have found difficult. Consequently, this feature’s MoSCoW prioritisation is classified as “Must Have.”

#### MoSCoW: MUST HAVE

This feature is considered a “Must Have” as it directly supports the client’s core requirements and objectives.

### 2.2.5 SFX

#### User Story

As a player, I want to have music and sound effects in the game, so the game feels more immersive and enjoyable.

#### Feature

* SFX

#### BR 5.1 Scenario: background music for immersive atmosphere

- Given the player is in a game level
- When the level starts
- Then background music plays that matches the mood and intensity of the level
- And the player can adjust the volume in settings

#### BR 5.2 Scenario: sound effects for actions

- Given the player performs an action
- When the action occurs
- Then a corresponding sound effect plays to reflect the action
- And the sound effect volume is consistent with the settings

#### BR 5.3 Scenario: adaptive music for boss encounters

- Given the player is entering a boss encounter
- When the encounter begins
- Then the music dynamically shifts to a more intense track
- And the music fades back to normal when the boss is defeated

#### BR 5.4 Scenario: environmental sound effects

- Given the player is moving through different environments
- When the player enters a new environment
- Then ambient sound effects play to match the setting

#### Rationale

Implementing this feature would contribute significantly to the client’s goal of creating an enjoyable and immersive game.
The inclusion of sound effects (SFX) enables another dimension to the game which can deepen the player’s immersion and enhance the game’s atmosphere. Despite not being explicitly stated in the Product/Service Requirements, it provides a great addition to the experience and greatly increases the high quality immersion the game seeks to achieve. For these reasons, this feature’s MoSCoW prioritisation is classified as “Should Have.”

#### MoSCoW: SHOULD HAVE

This feature is considered a “Should Have” as it adds value to the player’s experience but is not essential or equal in importance to the game’s core visual components.

### 2.2.6 Stylised Cold War era graphics

#### User Story

As a player, I want to experience the tension and politics of the Cold War era by being immersed in the atmosphere of that era.

#### Feature

* Stylised art

#### BR 6.1 Scenario: Cold War era background & atmosphere

- Given the player is exploring the Cold War era military base
- When they move through the map
- Then The background colours feature desaturated blues and greys contrasted by bright red soviet propaganda posters

#### BR 6.2 Scenario: enemy uniforms

- Given An enemy is in the player’s line of sight
- When the enemy renders on the screen
- Then the enemy is shown to be wearing dark militaristic uniform
- And their uniform is contrasted by bright red Soviet Union insignias
- And the player is able to identify guards easily

#### BR 6.3 Scenario: entering a room

- Given the player enters a room
- When they move around in the room
- Then the player is greeted by dim overhead lights which cast a glow beneath them
- And lights up any objects seen beneath the light

#### Rationale

Implementing this feature would contribute significantly to the client’s goal of creating a Cold War-inspired game.
The client’s Product/Service Requirements emphasise the importance of a Cold War stealth theme. Incorporating Cold War-era graphics would enhance the game’s atmosphere and strengthen its connection to the desired theme, aligning with the client’s vision. As this feature directly supports the core thematic focus of the game, its inclusion is essential. For these reasons, this feature’s MoSCoW prioritisation is classified as “Must Have.”

#### MoSCoW: MUST HAVE

This feature is considered a “Must Have” as it is crucial to achieving the game’s intended atmosphere and aligning with the client’s specific requirements.

### 2.2.7 Mission system

#### User Story

As a player, I want to complete mission objectives in a clear, structured storyline so there is a clear path of what to do next.

#### Feature

* Mission system

#### BR 7.1 Scenario: receiving mission objective

- Given the player has completed their previous mission
- When the player enters their mission logs
- Then the player is assigned a new mission
- And they learn about the objectives for the next mission

#### BR 7.2 Scenario: unlocking a reward for completing a mission

- Given the player has successfully completed a mission
- When they open their inventory
- Then the player unlocks a new item
- And the player can use this item for further missions

#### BR 7.3 Scenario: unlocking new chapter

- Given the player completes a major mission
- When they collect their reward
- Then a new cutscene plays
- And a new part of the map is unlocked

#### BR 7.4 Scenario: completing secondary objective

- Given a secondary objective has been completed during a mission
- When the mission ends
- Then the player is given a bonus reward

#### Rationale

Implementing this feature would enhance the client’s goal of creating a Cold War infiltration stealth game.
While a mission system is not a core requirement in the Product/Service Requirements, the document emphasises creating an immersive and thematic player experience. Including a mission system would help this by providing players with a structured and focused gameplay experience, aligning with the mindset of a Cold War spy. By offering clear progression via a way to track how much a player has completed and giving visual milestones in the Mission’s UI, this feature would deepen the player’s drive to continue playing. Therefore, it enhances their engagement with the game. For these reasons, this feature’s MoSCoW prioritisation is classified as “Could Have.”

#### MoSCoW: COULD HAVE

This feature is considered a “Could Have” as it is not essential but offers noticeable improvements to the player’s experience and alignment with the game’s theme.

### 2.2.8 Movement mechanics

#### User Story

As a player, I want to have basic movement mechanics, so I can navigate the map and complete missions.

#### Feature

* Movement

##### BR 8.1 Scenario: horizontal movement

- Given the player is moving their character
- When the player presses a sideward movement key
- Then the character should move in the corresponding direction
- And the character has a fluid walking animation

#### BR 8.2 Scenario: sprinting

- Given the player is moving
- When the player holds the sprint button
- Then the player’s speed increases by a large margin
- And the player’s stamina decreases while they sprint
- And when their stamina runs out their speed slows down

#### BR 8.3 Scenario: vertical movement

- Given the player is standing or moving on a solid surface
- When the player presses the spacebar
- Then then the character moves upwards in the air
- And the player’s character reaches peak height
- And the character starts moving downwards

#### BR 8.4 Scenario: dodge

- Given the player is moving while engaging an enemy
- When the player presses the dodge button
- Then the player performs a quick roll on the ground
- And evades an attack

#### BR 8.5 Scenario: fall damage

- Given the player jumps off a platform
- When the player lands on a solid surface
- Then the player takes fall damage if the height exceeded the fall damage limit
- And the player’s screen displays visual feedback

#### Rationale

Implementing this feature would significantly contribute to the client’s overall objective of creating an immersive and engaging player experience.
Movement mechanics allow players to navigate the game maps and scenarios effectively, providing the foundation for other key features such as stealth. The game can fully deliver on its intended Cold War espionage theme and ensure a cohesive gameplay experience by enabling these mechanics. For these reasons, this feature’s MoSCoW prioritisation is classified as “Must Have.”

#### MoSCoW: MUST HAVE

This feature is considered a “Must Have” as it is fundamental to the game’s functionality and directly supports the client’s objective of delivering an immersive stealth experience.

### 2.2.9 Combat mechanics

#### User Story

As a player, I want to have combat mechanics so I can defeat enemy guards while completing missions.

#### Feature

* Combat

#### BR 9.1 Scenario: shooting

- Given the player is engaging an enemy
- When the player’s bullet connects with the enemy
- Then the enemy takes damage based on strength of the weapon
- And the gun’s ammo decreases by one

#### BR 9.2 Scenario: enemy death

- Given the player shoots an enemy
- When the enemy’s health drop’s to or below 0
- Then the enemy is defeated
- And the enemy drops a coin

#### BR 9.3 Scenario: melee attack

- Given the player is next to an enemy
- When the player melee attacks the enemy
- Then the enemy is immediately defeated
- And the enemy drops two coins

#### BR 9.4 Scenario: out of ammo

- Given the player’s gun has run out of ammo
- When the player clicks the reload key
- Then the player reloads their weapon
- And can shoot again with full ammo

#### BR 9.5 Scenario: grenade attack

- Given player is holding a grenade
- When the player throws the grenade
- Then the grenade explodes upon impacting the ground
- And enemies within the blast radius take damage

#### BR 9.6 Scenario: player taking a hit

- Given the player is engaged in combat with an enemy
- When the player takes damage
- Then the player’s health points decrease according to how much damage they took
- And the player’s screen shows visual feedback

#### Rationale

Implementing this feature would significantly contribute to the client’s goal of enhancing gameplay and immersion within the themes of the Cold War and infiltration.
This feature allows players to engage in infiltration and combat scenarios to achieve their objectives, aligning closely with the game’s desired theme. However, due to its secondary nature compared to the primary stealth mechanic, this feature is not considered of higher priority. For these reasons, this feature’s MoSCoW prioritisation is classified as “Should Have.”

#### MoSCoW: SHOULD HAVE

This feature is considered a “Should Have” as it supports the game’s overall themes and objectives and enhances active gameplay elements. However, it is not essential for achieving the core gameplay experience.

# 3 - Project Management

## 3.1 - Risks and Issues

When developing a large software project with a team, it is important to be
aware of the potential risks and issues that may arise, and to have plans in
place to mitigate them. Issues may arise from a variety of sources, including
the group itself, the client, the software produced or even the available
hardware.

### 3.1.1 Group Risks

One of the most significant risks when working in a team is that of team members
not working together effectively. This may result from differing levels of
knowledge, different problem solving styles, alternate programming styles and
more. Fortunately, this can be mitigated by creating comprehensive plans for the
project and ensuring everyone is assigned roles that suit their strengths. If
the team cannot work together effectively, it can become difficult to develop a
cohesive product, and may lead to more bugs and issues down the line.

$$
\begin{aligned}
  &\text{Consequence}=4 \\
  &\text{Likelihood}=2 \\
  &\text{Risk}=4\times2=8 \implies {\color{red}\text{High}}
\end{aligned}
$$

Uneven workloads, despite being unfair on those doing more work, can lead to
some members not understanding the current state of the project or how new
features should be implemented. This can make it hard to continue contributing
to the project, resulting in even lower productivity, slower development and a
less complete product. To mitigate this risk, it is important to ensure that
everyone is assigned tasks before the project starts, and that regular meetings
are held to discuss progress and potential task shifts.

$$
\begin{aligned}
  &\text{Consequence}=3 \\
  &\text{Likelihood}=3 \\
  &\text{Risk}=4\times2=9 \implies {\color{red}\text{High}}
\end{aligned}
$$

### 3.1.2 Client Risks

Another area of concern is the potential for the client to provide unrealistic,
unreasonable or poorly defined requirements. If this happens, it can extremely
difficult to produce a cohesive plan and deliver a product that meets the
client's expectations. To mitigate such a risk, it is important to have multiple
meetings with the client at regular intervals, discussing the project's current
state, planned development and any changes the client may request. This way, the
client can provide feedback and desired alterations before they are too deeply
embedded in the project, or even before they are implemented.

$$
\begin{aligned}
  &\text{Consequence}=3 \\
  &\text{Likelihood}=2 \\
  &\text{Risk}=3\times2=6 \implies {\color{orange}\text{Moderate}}
\end{aligned}
$$

Another potential risk is that the client may be slow to respond, or completely
unresponsive. In this case, it can be difficult to get started on the project
because the requirements are not clearly defined, and it is difficult to provide
a product that meets their expectations, since the feedback is not beign
given in a timely manner. To mitigate this risk, it is important to have a clear
communication plan with set dates and deadlines for features, meetings and
general updates. It is also especially important to develop a well-written
codebase which can be easily modified and maintained.

$$
\begin{aligned}
  &\text{Consequence}=2 \\
  &\text{Likelihood}=1 \\
  &\text{Risk}=2\times1=2 \implies {\color{green}\text{Low}}
\end{aligned}
$$

### 3.1.3 Software Development Methodology Risks

Large codebases require strict adherence to project-defined standards, as
otherwise it can become difficult for team members to work on other parts of the
project. Additionally, the code must be highly modular, since the code developed
by one team member must be able to interact with the code developed by others,
regardless of the implementation details. Furthermore, it is useful to have
clean, readable code, with comments in areas that are not obvious. This ensures
that code can still be understood months after it was first written, which may
be necessary for debugging and maintenance purposes.

$$
\begin{aligned}
  &\text{Consequence}=4 \\
  &\text{Likelihood}=4 \\
  &\text{Risk}=4\times4=16 \implies {\color{orange}\text{Extreme}}
\end{aligned}
$$

### 3.1.4 AI Risks

AI is a very powerful tool, but can be dangerous if not used correctly. LLMs in
particular can be very useful and produce high quality results, but are also
susceptible to biases and may produce inaccurate or inappropriate results.
Fortunately, much research has been conducted in this area, and popular LLMs are
'aligned' with human values, and are highly unlikely to produce offensive
suggestions. Unfortunately, incorrect results are still quite common.

$$
\begin{aligned}
  &\text{Consequence}=5 \\
  &\text{Likelihood}=1 \\
  &\text{Risk}=5\times1=5 \implies {\color{orange}\text{Moderate}}
\end{aligned}
$$

## 3.2 - Development Approach

### 3.2.1 Incremental Model

* The Incremental Model divides the project into smaller, manageable portions (increments). Each increment represents a subset of the full game functionality.
* With each increment, you develop a part of the game and deliver it. Once that part is functional, you can move on to the next one.
* Each new increment builds upon the previous one, allowing you to add new features, mechanics, or puzzle elements step by step.
* Risk is managed effectively by focusing on simpler, high-priority features early and addressing complex ones later.

#### 3.2.2 Application in the Project Context

1. **Gradual Progress**: Our game comprises multiple components, including core mechanics (e.g., running, jumping, and sliding), puzzles, and IBM Skills Build badge questions. Using the Incremental Model, we can develop each component sequentially, starting with the foundational mechanics before progressing to puzzles and gamification elements like the card system.
2. **Iterative Feedback and Testing**: After completing each increment, we can conduct testing and gather client or player feedback. This ensures each feature works as intended and aligns with the project goals before integrating it into the overall game.
3. **Risk Management**: By addressing straightforward features early in the development process, the Incremental Model reduces risk and ensures a consistent development pace. This strategy allows us to allocate more time and resources to resolving challenges posed by complex features.
4. **Flexibility for Changes**: Given the possibility of evolving client requirements or necessary adjustments, the Incremental Model minimizes disruption. Changes can be accommodated without requiring significant rework, as only specific increments need updating, preserving the integrity of prior developments.
5. **Early Player Engagement**: The model allows us to release a playable version of the game before full completion. Early releases enable us to collect valuable feedback from players and the client, refining the game’s direction and functionality based on real-world input.
6. **Adaptability to a Small Team**: With a team of seven, the Incremental Model facilitates efficient workload distribution. Team members can focus on specific tasks within each increment, ensuring all participants are engaged and progress is clearly tracked.
7. **Simplicity and Structure**: The model offers a straightforward and predictable process, breaking development into clear, manageable stages. This structured approach ensures transparency and provides measurable milestones throughout the project lifecycle.

## 3.3 - Project Schedule

### 3.3.1 Gantt Chart

<p align="center">
    <img src="./images/gantt-chart.png">
</p>

### 3.3.2 Art / Vision Design

Due to **Unity** offering an open source template that closely
matches the client's vision of the final game we have decided to
have an extended period of planning the aspects of the game such as:

Game mechanics which include: stealth mechanics, boss fights (as per request of the client),
integration of IBM's Granite AI into the project which would make game feel more "alive" as
it adapts to the player's behaviour, and lastly the educational part of the game
which integrates content from IBM's SkillsBuild.

Careful thought has to go to the game's art direction, while the client
specified CounterSpy as an inspiration he has allowed us to have artistic liberty
which allows us to choose a style that could mesh together all of the client's
requests.

### 3.3.3 RAG Analysis and the Questions Bank

As mentioned in `3.3.2` the game has an educational side and the client
has tasked us with taking questions from the IBM SkillsBuild courses
and integrating them into the game.

This is one of the first things that need to be completed in order
to have a clear vision over how the game should take shape.

### 3.3.4 C# / Unity Acquaintance

Not all of our team is well acquainted with the `C#` / `Unity` environment,
because of this we have planned a long lasting period (until the end of Christmas
Vacation) where we can familiarize ourselves with the toolset.

For the team members who already have experience, there is a period
dedicated to refactoring the Unity template in order to better suit our needs.

### 3.3.5 Game Mechanics Implementation and Test Plan

The Game Mechanics Implementation and preparation of the Test Plan
will run in parallel, as we develop the game incrementally we
can identify ways of testing our solution.

Key mechanics that have to be implemented and polished are:

1) Basic movement: movement should feel fluid, responsive, must include
   crouching (stealth).
2) Stealth Mechanics: "light" and "dark" zones where the AI of
   the laboratory can either see you or not, if detected sends enemies
3) The environment reacting to the player: by using IBM's Granite AI
   we hope to achieve enemies/bosses that react dynamically to the player;
   and even in the case of the educational side make the questions harder/easier
   depending on the success rate of the player.

### 3.3.6 Level Design and IBM Education Integration

Once we have a solid foundation of game mechanics we expect
to be able to create a plausible environment around said mechanics.

We plan on having `3` different stages due to how the **Questions Bank**
is based off of 3 different IBM SkillsBuild courses.

Each stage would teach the player different game mechanics and introduce
new concepts.

### 3.3.7 Technical Report and Product Presentation

They will start to run in parallel once we are at the end of the development
cycle with the **Game Mechanics Implementation**

### 3.3.8 Key Deadlines

* `2024-11-28`: Finish client RAG Analysis and Questions Bank
* `2024-03-21`: Finish all game deadlines (mechanics, level design, IBM integration)
