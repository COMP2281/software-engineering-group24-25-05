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
Version: 1.0.2

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

To refine our requirements into behavioral specifications, we used Gherkin language. For example, we created scenarios detailing how the game would present challenges, adjust difficulty dynamically, and integrate IBM Skills Build content seamlessly. This method allowed us to concisely capture the desired system behavior and align it with the client’s educational objectives. This allowed us to create user stories that detailed the player’s journey through the game, from initial engagement to mastery of key concepts, ensuring that our requirements were user-focused and aligned with the client’s vision. **Don't like this bit, feel like I should just use more Gherkin language throughout**

**Requirement Validation**
To ensure that our refined requirements aligned accurately with the client’s needs, we sent a confirmation email summarizing key decisions and requested feedback. The client’s positive response affirmed our direction. We also planned a follow-up meeting to further validate our approach and address any remaining questions, ensuring continuous alignment with the client’s educational goals.

Our team then met to discuss the client’s feedback and refine our understanding of the project’s requirements. We identified key features, such as question generation, dynamic difficulty adjustment, and boss interactions, that would enhance the game’s educational value and align with the client’s objectives.

## 2.2 - Behavioural Requirements

# 3 - Project Management

## 3.1 - Risks and Issues

## 3.2 - Development Approach

## 3.3 - Project Schedule
