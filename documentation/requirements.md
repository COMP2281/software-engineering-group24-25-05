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
* Date the document was prepared:
* Version: 1.0.2


# 1 - Introduction

## 1.1 - Overview and Justification

The purpose of this project is to develop an engaging educational game, based on the CounterSpy universe, that integrates IBM Skills Build content into its gameplay. This approach transforms traditional educational content into an immersive, game-based experience that enhances learner engagement and retention.

It will benefit individuals pursuing IBM Skills Build knowledge, with potential applications for educational institutions or self-learners interested in complementing their studies with an engaging, interactive tool.

The project’s client is IBM, represented by John McManara, who leads IBM UK University Programs and serves as an IBM Master Inventor. IBM aims to make IBM Skills Build content more accessible and engaging through this game.

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

## 1.3 - System Description

**System Overview**

Shadow Operative is an educational game designed to help learners engage with IBM Skills Build content in an immersive and interactive environment. The game places players in the role of a spy during the Cold War, tasked with retrieving stolen plans for a cutting-edge AI system. Through skill-based challenges and AI-driven adaptive difficulty, players are guided through progressively complex levels that reinforce core IBM Skills Build concepts in a gamified format.

**Research into Alternative Solutions**

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

**Proposed Solution’s Technical Features**

Shadow Operative integrates the following key technical features to create a unique, adaptive learning experience:

- **Question Generation**: Relevant IBM Skills Build content will populate various levels, embedding knowledge assessments seamlessly into gameplay scenarios. Questions will adapt in difficulty and context to align with players' progress, enhancing engagement and reinforcing learning objectives.
- **Dynamic Difficulty Adjustment**: AI algorithms will monitor and assess player performance, adjusting challenge levels in real-time. This ensures players are consistently engaged and challenged without being overwhelmed, fostering a tailored learning experience.
- **Boss Interactions**: Key encounters, such as boss battles, act as knowledge assessments, where players demonstrate their understanding of core Skills Build concepts in a high-stakes, interactive format that reinforces learning.
- **Stealth Mechanics**: Inspired by CounterSpy and Dishonored 2, Shadow Operative incorporates stealth elements like sneaking, cover mechanics, and enemy awareness to create a tense, immersive experience that complements the game's educational content, keeping the user engaged.

**Integration Considerations**

Shadow Operative is designed as a standalone educational game; however, it is making use of IBM’s AI technology, Granite, allowing the user to make the experience adpative to them, focusing more on their usecase of the system.

# 2 - Solution Requirements

## **2.1 Requirements Elicitation**

To gain a thorough understanding of the client’s needs, our team reached out via email on 15/10/24, aiming to address specific concerns about feature integration, gameplay mechanics, and AI-driven educational content. The client responded on 17/10/24, clarifying the project’s focus on seamlessly incorporating IBM Skills Build content (including AI, Cybersecurity, and Data Analytics) into the gameplay. This exchange allowed us to refine the project scope, confirm the inclusion of adaptive learning through AI algorithms, and remove features identified as redundant. These insights also guided us in structuring team roles based on each member’s strengths and development areas.

**Challenges Encountered**

Our initial difficulty was organizing our first meeting with the client due to conflicting schedules among team members. This caused a delay in setting up a direct conversation, which impacted our early progress. Additionally, one of our team members was unavailable for a few weeks due to being away, which further complicated scheduling and resource allocation. Despite these obstacles, the team coordinated through asynchronous communication and adjusted roles temporarily to maintain momentum.

Following this, we held an internal meeting to discuss our objectives in depth and to identify how each member’s skills could contribute to creating a compelling, interactive experience that aligns with CounterSpy’s Cold War-inspired, side-scrolling stealth gameplay. This meeting helped us generate additional questions for the client regarding elements such as how best to implement game mechanics that motivate players to learn and progress through adaptive, question-based challenges. We also explored design inspirations, focusing on essential aspects like gameplay mechanics, Cold War-era visual style, and strategies to maintain a consistent thematic experience. During this meeting, we reached a consensus that the card feature did not align with the game’s objectives, which we then proposed for removal in a follow-up email. The client agreed with this decision.

In our first client meeting on 06/11/24, we discussed key project concerns and shared a preliminary MoSCoW prioritization to confirm alignment with the client’s goals. The client agreed that the educational component should be the top priority, as the primary aim is to promote IBM Skills Build badges in a more engaging way than current text-based modules. To address this, we collaboratively designed a core game mechanic: when a player is spotted by a robotic guard, they must answer a multiple-choice question to proceed.

To refine our requirements into behavioral specifications, we used Gherkin language. For example, we created scenarios detailing how the game would present challenges, adjust difficulty dynamically, and integrate IBM Skills Build content seamlessly. This method allowed us to concisely capture the desired system behavior and align it with the client’s educational objectives. This allowed us to create user stories that detailed the player’s journey through the game, from initial engagement to mastery of key concepts, ensuring that our requirements were user-focused and aligned with the client’s vision.

**Requirement Validation**

To ensure that our refined requirements aligned accurately with the client’s needs, we sent a confirmation email summarizing key decisions and requested feedback. The client’s positive response affirmed our direction. We also planned a follow-up meeting to further validate our approach and address any remaining questions, ensuring continuous alignment with the client’s educational goals.

Our team then met to discuss the client’s feedback and refine our understanding of the project’s requirements. We identified key features, such as question generation, dynamic difficulty adjustment, and boss interactions, that would enhance the game’s educational value and align with the client’s objectives.

## 2.2 - Behavioural Requirements



### User Stories:

### Stealth Gameplay 
	 
**User Story:** 

As a player, I want a side-scrolling experience where I can use stealth mechanics to infiltrate enemy bases, so that I can emulate spy tactics.

**Feature:** Stealth Mechanics

**BR1.1 Scenario:** Using Cover to Avoid Detection

- Given the player is near enemy guards,
- And the player is within the guard’s line of sight,
- When the player moves into cover (e.g vent, shadowed area, or behind an object),
- Then the guards should not detect the player.

**BR1.2 Scenario:** Making noise near Guards
- Given the player is making noise,,,
- When a guard is within earshot,
- Then then an investigation state should be triggered,
- And the guard should investigate the noise made.
- And if the guard detects the player, an alert state is triggered.

**BR1.3 Scenario: Crouch Walking to Avoid Detection**
- Given the player is outside any cover,
- And the player is within the  guard’s line of sight,
- When the player moves slowly in a crouched or stealthy stance,
- Then the guards should have a reduced chance of detecting the player,
- And the player can bypass guards without triggering an alert if they remain at a safe distance.

**BR1.4 Scenario:** Triggering an Alert When Spotted
- Given the player is within a visible range of the guard’s line of sight,
- And the player is not in cover,
- When the guard spots the player,
- Then an alert state should be triggered,
- And nearby guards should move toward the player’s last known position.

**BR1.5 Scenario:** Returning to Patrol after Losing Sight of Player
- Given guards are in an alert state after spotting the player,
- And the player has moved out of their line of sight,
- When the guards do not detect the player for a set amount of time,
- Then the guards should return to their normal patrol behaviour,
- And the alert state should end.

**Rationale**

Implementing this feature would contribute significantly to the client’s objectives as the core gameplay feature in the game design desired by the client was Stealth. 

Due to the theme of the game being Cold war espionage, stealth is one of the core requirements of the game outlined by the client, therefore implementing this feature would contribute significantly to the clients objective, therefore this feature’s MosCoW prioritisation is “Must Have”.

**MoSCoW Priority** : **MUST HAVE**





### Educational Engagement

**User Story:** 
As a player, I want to answer AI, Cybersecurity, and Data Analytics questions from the IBM Skills Question Bank, so that I can test my knowledge and understand IBM Skills Build concepts.

**Feature:** IBM Skills Question Bank

**BR2.1 Scenario:** Player steals classified document
- Given the player has found a classified document,
- When the player steals the document ,
- Then the player must answer a question related to AI, Data Analytics, or Cybersecurity
- And if the player answers the question correctly, they get 10 coins
- But if the player answers the question incorrectly, guards nearby are triggered into an alert state.

**BR2.2 Scenario:** Quiz at the End of the Level 
- Given the player has reached the final challenge of the level,
- When the quiz prompt appears on the screen,
- Then the player must answer a series of questions related to AI, Data Analytics, and Cybersecurity,
- And if the player answers the questions correctly an accuracy of at least 80%, they can progress to the next level.
- But if the player answers incorrectly, they must redo the quiz.

**Rationale:**

The client aims to expand beyond their pre-existing website by exploring new mediums to attract more interest in IBM Skills Build. 
This feature would ensure that the game’s design aligns with the client’s educational goals, seamlessly integrating the learning aspect into the gameplay, creating a fun and engaging platform to develop IBM Skills Build. As such, this feature has been assigned a MoSCoW prioritisation of “Must Have.”

**MoSCoW Priority: MUST HAVE**



### Pause Screen
**User Story:** 
As a player, I want to be able to pause my game so that I can take a break from the game, change the settings or exit the game.

**Feature:** Pause Screen

**Background:**
- Given the player is in a level
- When the player presses the designated pause key on the keyboard
- Then the game pauses
- And the Pause screen is displayed
- And buttons are displayed on the screen 

**BR3.1 Scenario:** Taking a break from the game
- Given the game is paused
- When the player clicks the resume button 
- Then the game resumes 
- And the player can pick up where they left off  

**BR3.2 Scenario:** Accessing and Adjusting Game Settings from the Pause Screen
- Given the game is paused
- When the player selects the settings option from the Pause screen
- Then the settings menu is displayed
- And the player can adjust settings to their preference

**Rationale**

Implementing this feature would enhance the user’s gameplay experience and improve the overall quality of life within the game. Rather than having to complete the game in a single session, players would be able to pause the game, and complete it a later time. Although a pause screen is not explicitly mentioned as a core requirement, this feature adds flexibility and convenience to the game, therefore significantly improving the game’s quality of life, achieving the client’s objectives of a high quality user experience ; Due to the reasons outlined above, this feature’s MoSCoW prioritisation is classified as “Could Have.”

**MoSCoW: COULD HAVE**

This feature is considered a “Could Have” as it is not a mandatory requirement but offers a meaningful improvement to the player experience.


### AI Boss
**User Story:** 
As a player, I want an AI boss that challenges the skills I learnt throughout the game, so that I can demonstrate my abilities in stealth, quizzes and combat mechanics under pressure.
**Feature:** Adaptive Learning

**BR4.1 Scenario:** Testing Stealth Skills
- Given the player has demonstrated weakness in Stealth based tasks in the game
- When they encounter the final boss
- Then the boss will adapt the the level environment to include stealth based mechanics 
- But the main objective is still the final quiz  

**BR4.2 Scenario:** Quiz adjustment 
- Given the player has previously struggled with certain topics.
- When they encounter the final boss
- Then the boss can adapt to the player’s weaknesses 
- And generate a custom quiz for the player 
- And include the player’s weaknesses in the custom quiz 

**BR4.3 Scenario:** Combat mechanics adjustment
- Given the player has demonstrated weakness in combat abilities
- When the player encounters the final boss
- Then can adjust the level to include to include an element of combat based mechanics alongside the final quiz
- But the main objective is still the final quiz 

**Rationale**

Implementing this feature would significantly contribute to the client’s overall goals and objectives, as AI algorithms were explicitly specified in the Technical Requirements of the Product/Service Requirements.
The use of AI to personalise the learning experience and adapt the game to the player’s performance is a key directive from the client. By incorporating this feature, the game ensures alignment with the client’s vision and technical specifications. Whilst also additionally building upon the education aspect of the game, enabling the user to be tested and learn based on what it may have found difficult. Consequently, this feature’s MoSCoW prioritisation is classified as 
“Must Have.”

**MoSCoW: MUST HAVE**

This feature is considered a “Must Have” as it directly supports the client’s core requirements and objectives.

### SFX
**User Story:** 
As a player, I want to have music and sound effects in the game, so the game feels more immersive and enjoyable.
**Feature:** SFX

**BR5.1 Scenario:** Background Music for Immersive Atmosphere
- Given the player is in a game level
- When the level starts
- Then background music plays that matches the mood and intensity of the level
- And the player can adjust the volume in settings

**BR5.2 Scenario:** Sound effects for actions
- Given the player performs an action (e.g., shooting, opening doors, or collecting items)
- When the action occurs
- Then a corresponding sound effect plays to reflect the action
- And the sound effect volume is consistent with the settings

**BR5.3 Scenario:** Adaptive Music for Boss Encounters
- Given the player is entering a boss encounter
- When the encounter begins
- Then the music dynamically shifts to a more intense track
- And the music fades back to normal when the boss is defeated

**BR5.4 Scenario:** Environmental Sound Effects
- Given the player is moving through different environments 
- When the player enters a new environment
- Then ambient sound effects play to match the setting

**Rationale** 

Implementing this feature would contribute significantly to the client’s goal of creating an enjoyable and immersive game.
The inclusion of sound effects (SFX) enables another dimension to the game which can deepen the player’s immersion and enhance the game’s atmosphere. Despite not being explicitly stated in the Product/Service Requirements, it provides a great addition to the experience and greatly increases the high quality immersion the game seeks to achieve. For these reasons, this feature’s MoSCoW prioritisation is classified as “Should Have.”

**MoSCoW: SHOULD HAVE**

This feature is considered a “Should Have” as it adds value to the player’s experience but is not essential or equal in importance to the game’s core visual components.


### Stylised Cold War era graphics   
	
**User story:**
As a player, I want to experience the tension and politics of the Cold War era by being immersed in the atmosphere of that era.

**Feature:** Stylised art 

**BR6.1 Scenario:** Cold War era background & atmosphere
- Given the player is exploring the Cold War era military base
- When they move through the map 
- Then The background colours feature desaturated blues and greys contrasted by bright red soviet propaganda posters

**BR6.2 Scenario:** Enemy uniforms 
- Given An enemy is in the player’s line of sight 
- When the enemy renders on the screen
- Then the enemy is shown to be wearing dark militaristic uniform
- And their uniform is contrasted by bright red Soviet Union insignias 
- And the player is able to identify guards easily 

**BR6.3 Scenario:** Entering a room
- Given the player enters a room 
- When they move around in the room  
- Then the player is greeted by dim overhead lights which cast a glow beneath them
- And lights up any objects seen beneath the light

**Rationale**

Implementing this feature would contribute significantly to the client’s goal of creating a Cold War-inspired game.
The client’s Product/Service Requirements emphasise the importance of a Cold War stealth theme. Incorporating Cold War-era graphics would enhance the game’s atmosphere and strengthen its connection to the desired theme, aligning with the client’s vision. As this feature directly supports the core thematic focus of the game, its inclusion is essential. For these reasons, this feature’s MoSCoW prioritisation is classified as “Must Have.”

**MoSCoW: MUST HAVE**

This feature is considered a “Must Have” as it is crucial to achieving the game’s intended atmosphere and aligning with the client’s specific requirements.


### Mission system  
**User story:** 
As a player, I want to complete mission objectives in a clear, structured storyline so there is a clear path of what to do next.
**Feature:** Mission system   

**BR7.1 Scenario:** Receiving mission objective 
- Given the player has completed their previous mission
- When the player enters their mission logs  
- Then the player is assigned a new mission  
- And they learn about the objectives for the next mission    

**BR7.2 Scenario:** Unlocking a reward for completing a mission 
- Given the player has successfully completed a mission  
- When they open their inventory 
- Then the player unlocks a new item   
- And the player can use this item for further missions 

**BR7.3 Scenario:** Unlocking new chapter 
- Given the player completes a major mission 
- When they collect their reward 
- Then a new cutscene plays 
- And a new part of the map is unlocked

**BR7.4 Scenario:** Completing secondary objective  
- Given a secondary objective has been completed during a mission 
- When the mission ends 
- Then the player is given a bonus reward

**Rationale**

Implementing this feature would enhance the client’s goal of creating a Cold War infiltration stealth game.
While a mission system is not a core requirement in the Product/Service Requirements, the document emphasises creating an immersive and thematic player experience. Including a mission system would help this by providing players with a structured and focused gameplay experience, aligning with the mindset of a Cold War spy. By offering clear progression via a way to track how much a player has completed and giving visual milestones in the Mission’s UI, this feature would deepen the player’s drive to continue playing. Therefore, it enhances their engagement with the game. For these reasons, this feature’s MoSCoW prioritisation is classified as “Could Have.”

**MoSCoW: COULD HAVE**

This feature is considered a “Could Have” as it is not essential but offers noticeable improvements to the player’s experience and alignment with the game’s theme.

### Movement mechanics 
**User story:** As a player, I want to have basic movement mechanics, so I can navigate the map and complete missions.

**Feature:** Movement

**BR8.1 Scenario:** Horizontal movement 
- Given the player is moving their character 
- When the player presses a sideward movement key 
- Then the character should move in the corresponding direction 
- And the character has a fluid walking animation 

**BR8.2 Scenario:** Sprinting 
- Given the player is moving 
- When the player holds the sprint button 
- Then the player’s speed increases by a large margin 
- And the player’s stamina decreases while they sprint 
- And when their stamina runs out their speed slows down  

**BR8.3 Scenario:** Vertical movement 
- Given the player is standing or moving on a solid surface 
- When the player presses the spacebar 
- Then then the character moves upwards in the air  
- And the player’s character reaches peak height 
- And the character starts moving downwards 

**BR8.4 Scenario:** Dodge 
- Given the player is moving while engaging an enemy 
- When the player presses the dodge button
- Then the player performs a quick roll on the ground 
- And evades an attack

**BR8.5 Scenario:** Fall damage
- Given the player jumps off a platform 
- When the player lands on a solid surface 
- Then the player takes fall damage if the height exceeded the fall damage limit 
- And the player’s screen displays visual feedback

**Rationale**

Implementing this feature would significantly contribute to the client’s overall objective of creating an immersive and engaging player experience.
Movement mechanics allow players to navigate the game maps and scenarios effectively, providing the foundation for other key features such as stealth. The game can fully deliver on its intended Cold War espionage theme and ensure a cohesive gameplay experience by enabling these mechanics. For these reasons, this feature’s MoSCoW prioritisation is classified as “Must Have.”

**MoSCoW: MUST HAVE**

This feature is considered a “Must Have” as it is fundamental to the game’s functionality and directly supports the client’s objective of delivering an immersive stealth experience.

### Combat mechanics 
**User story:** 

As a player, I want to have combat mechanics so I can defeat enemy guards while completing missions.

**Feature:** Combat 

**BR9.1 Scenario:** Shooting 
- Given the player is engaging an enemy 
- When the player’s bullet connects with the enemy  
- Then the enemy takes damage based on strength of the weapon 
- And the gun’s ammo decreases by one 

**BR9.2 Scenario:** Enemy death
- Given the player shoots an enemy 
- When the enemy’s health drop’s to or below 0 
- Then the enemy is defeated 
- And the enemy drops a coin 

**BR9.3 Scenario:** Melee attack 
- Given the player is next to an enemy 
- When the player melee attacks the enemy 
- Then the enemy is immediately defeated 
- And the enemy drops two coins 

**BR9.4 Scenario:** Out of ammo 
- Given the player’s gun has run out of ammo 
- When the player clicks the reload key 
- Then the player reloads their weapon 
- And can shoot again with full ammo 

**BR9.5 Scenario:** grenade attack
- Given player is holding a grenade 
- When the player throws the grenade
- Then the grenade explodes upon impacting the ground
- And enemies within the blast radius take damage 

**BR9.6 Scenario:** Player taking a hit
- Given the player is engaged in combat with an enemy  
- When the player takes damage 
- Then the player’s health points decrease according to how much damage they took 
- And the player’s screen shows visual feedback

**Rationale**

Implementing this feature would significantly contribute to the client’s goal of enhancing gameplay and immersion within the themes of the Cold War and infiltration.
This feature allows players to engage in infiltration and combat scenarios to achieve their objectives, aligning closely with the game’s desired theme. However, due to its secondary nature compared to the primary stealth mechanic, this feature is not considered of higher priority. For these reasons, this feature’s MoSCoW prioritisation is classified as “Should Have.”

**MoSCoW: SHOULD HAVE**

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
  &\text{Risk}=4\times2=8 \implies {\color{orange}\text{High}}
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
  &\text{Risk}=4\times2=9 \implies {\color{orange}\text{High}}
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
  &\text{Risk}=2\times1=2 \implies {\color{orange}\text{Low}}
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
