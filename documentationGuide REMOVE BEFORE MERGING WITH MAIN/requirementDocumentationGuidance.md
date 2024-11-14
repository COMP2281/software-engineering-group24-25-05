**Summative Assignment**

| **Module code and title**    | COMP2281 Software Engineering (Team work) |
| :--------------------------- | :---------------------------------------- |
| **Academic year**            | 2024-25                                   |
| **Coursework title**         | Requirement Documentation                 |
| **Coursework credits**       | 2\.2 credits                              |
| **% of module’s final mark** | 11%                                       |
| **Lecturer**                 | Eamonn Bell                               |
| **Submission date\***        | Friday, November 22, 2024 14:00           |
| **Estimated hours of work**  | 4\.4 hours                                |
| **Submission method**        | Ultra                                     |

| **Additional coursework files**                            | _None_                                                                                                                                                                                                                                                                  |     |
| :--------------------------------------------------------- | :---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | :-- |
| <p>**Required submission items**</p><p>**and formats**</p> | <p>_Requirement Documentation in PDF format._</p><p></p><p>_The filename must have the following structure: RequirementDocumentation_GroupNN.pdf_ </p><p></p><p>_where NN = your SE group number, zero-padded (e.g. Group 1 -> Group01, Group 21 -> Group21, etc.)_</p> |     |

\* This is the deadline for all submissions except where an approved extension is in place.

Late submissions received within 5 working days of the deadline will be capped at 40%.

Late submissions received later than 5 days after the deadline will receive a mark of 0.

It is your responsibility to check that your submission has uploaded successfully and obtain a submission receipt.

Your work must be done by yourself (or your group, if there is an assigned groupwork component) and comply with the university rules about plagiarism and collusion. Students suspected of plagiarism, either of published or unpublished sources, including the work of other students, or of collusion will be dealt with according to University guidelines (<https://www.dur.ac.uk/learningandteaching.handbook/6/2/4/>).
\*\*

COMP2281: SOFTWARE ENGINEERING 2023/24

# Requirements Documentation - Guidance

# General Guidance

The objective of this coursework is to assess your understanding and application of the principles of requirements elicitation, relevant software development methodologies, risk management, and planning.

You **must** produce a maximum 15-page (A4, 11pt font size) **Requirements Document**, which specifies the proposed behaviour of your group’s solution to your client’s requirements, your chosen software development methodology, and your project schedule for the duration of the project.

You **must** conform to the structure below. You should try to ensure that the document has been developed as the result of oral or written discussion with your client, all your group members, and any other stakeholders, or otherwise reflects your best understanding of their requirements (e.g. by role-playing client representatives in group meetings).

You **must** organise your document according to the following guidance. Note that 10% of the marks are for writing skills, the clarity of the document submitted, and consistent formatting/presentation.

You **must** the numbered headings provided here to structure your document. The document should be suitable for reference use in a professional setting, serving several purposes:

- to communicate and verify your understanding of the client’s requirements
- to serve as a reference of the client’s requirements for your software engineering team during the coming year
- to communicate your understanding of the client’s requirements and your proposed development approach to parties not involved in the requirements-gathering exercise (including hypothetical new hires on the project side and/or the client side, external stakeholders etc.)

# Relation to mark scheme

This guidance is intended to be read in conjunction with the mark scheme (below).

# Cover page

You must include a single cover page indicating:

- The title of the document (i.e. “Requirements Document for <Project Name>”)
- The authors of the document (i.e. full names and CIS usernames for each group member)
- The group number of the software engineering team
- The date the document was prepared
- Any other revision details as necessary (e.g. version number/information)

# 1 – Introduction

# 1\.1 - Overview and justification

Specify the purpose and need for the proposed system in high-level terms, and precisely who it will benefit if delivered as proposed. You should include details about your client, including any relevant organisational details (e.g. who your contact within the organisation is) and their aims and motivations. You should also briefly state how the remainder of the document is structured, including the rest of the introduction section.
Recommended ½ page.

# 1\.2 - Project scope

Specify the exact project scope, indicating the project’s boundaries. This should also include the purpose of the software project, your overall goals, and how these align with the interests of your and other stakeholders. This section should contain your vision for your product or service and should indicate the exact user base of the proposed product.

Recommended ½ page.

# 1\.3 - System description

You should provide an overview of the system to be built. Where appropriate, you should first briefly detail any existing/legacy systems. You should demonstrate what research into alternative solutions the team has undertaken by providing a brief description of comparable commercial or non-commercial solutions. You should evaluate the usefulness. In this section, you should focus on the technical features of the proposed solution itself, rather than on its behavioural requirements. Where these have not yet been determined, note that this is the case.

Recommended 1 page.

# 2 – Solution Requirements

This section should give insight into the outcome of your requirements-gathering discussions with your client and stakeholders along with evidence of your team’s efforts to ground the requirements in these discussions. You should provide evidence of the logical organisation and prioritisation of your requirements.

The approach you must use to specify the requirements of your client takes its inspiration from **behaviour-driven development** (BDD). Therefore, you must use User Stories and Gherkin (<https://cucumber.io/docs/gherkin/reference/>) to specify solution requirements as features and scenarios. The Gherkin pseudocode specifies how your system should behave in a concise, clear, and objective way; this makes it easy to assess whether the feature the pseudocode describes has been implemented. In turn, this allows stakeholders to understand when the underlying client requirements are fulfilled.

| **User Stories** are short texts that capture the needs of the users of your solution. **Gherkin** is a DSL (Domain Specific Language) that uses natural language (e.g. English) and a controlled vocabulary to specify **features** of your solution. Each feature must be written in valid Gherkin pseudocode (in English) and correspond to one User Story. For this assignment, each **feature** must consist of two or more **scenarios**. |
| :---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |

# 2\.1 - Requirements elicitation

Report on the steps undertaken by the group to elicit the client’s requirements, to develop them into User Stories, and to refine them into behavioural specifications using the Gherkin language. You should convince the reader that the specifications you have arrived at represent the requirements of the client to the best of your ability. Report on any difficulties or challenges you encountered and how you overcame them (if relevant). This section may mention any meetings or correspondence you had with the client, although screenshots of emails or DMs are not appropriate here. You can also report on the outcomes of any internal meetings that were especially influential on the refinement and validation of the requirements and specify any established methods or approaches you used to achieve this. Recommended 1 page

# 2\.2 - Behavioural requirements:

Provide numbered, behavioural requirement specifications for between 8 and 10 features of your proposed solution. Each scenario must be numbered such that (e.g.) BR1.2 refers to the second scenario in the first feature, BR3.1 refers to the first scenario in the third feature (etc.). For each specified feature, you must present:

- One **user story**, based on your requirement elicitation work.
- One valid **Gherkin pseudocode** listing (use a code or preformatted text block) expressing this user story as a **feature** consisting of at least two **scenarios**. You may choose to number the scenarios in Gherkin comments.
- A very brief **rationale** explaining how implementing this feature will contribute to the overall objectives of the client and its relative priority. You should use an established system to express this prioritisation (e.g. MoSCoW)

Maximum 10 pages (this implies approximately 1 page per specification)

# 3 – Project Management

# 3\.1 – Risks and Issues

Briefly identify and discuss any potential risks (or issues with the potential to become risks) that could possibly impact the project. This is a wide-ranging exercise and could include aspects of the group, the client, the chosen software development methodology, hardware, software, current systems etc. You should evaluate any problems that these could cause and propose appropriate mitigations. You should explicitly calculate (or otherwise compute) and prioritise risks using one of the techniques for assessing risk covered in lecture. Your mitigations should not be generic. Recommended 1 page.

# 3\.2 – Development approach

Discuss the software development lifecycle (SDLC) approach the group would like to use for this project. You should justify your reasons for selecting your approach(es) as opposed to others. Your justification should be grounded in the specifics of your project scope, team, client, and organisational capabilities, as opposed to generic claims for the effectiveness of a given approach. Recommended 1 page.

# 3\.3 – Project Schedule

Provide a plausible project schedule, clearly identifying academic and non-academic deadlines for key aspects of the project. You may also want to indicate the date and nature of other key milestones. This can be provided in the format you deem most suitable (e.g. a Gantt chart), but whatever format you choose should provide sufficient detail to organise the work of your team throughout the year. This should be easily readable and take note of the deadlines for the summative aspects of the project. Your schedule should go beyond a restatement of the academic deadlines. Recommended 1 page.
\*\*

# Requirements Specification - Mark Scheme

You will be assessed on how the well the requirements documentation meets each of the following objectives, with reference to the departmental assessment criteria. The Guidance (above) will inform the evaluation of each subsection.

# 1 – Introduction (20%)

# 1\.1 - Overview and justification (5%)

- Correctly and precisely identifies the client, their aims, motivations, and project goals.
- Describes the purpose of the system you are proposing
- Describes the structure of the remainder of the document.

# 1\.2 - Project scope (5%)

- Identifies the problems the proposed solution is aiming to solve.
- Identifies the purpose of the software and outline how it goes about solving the problems currently experienced by the client.
- Identifies users and other stakeholders of the proposed solution.

# 1\.3 - System description (10%)

- Provides details of the system design, including relevant technical details
- Provides evidence of research of existing or alternative solutions and why might they be appropriate or inappropriate in this case.
- Identifies the advantages and disadvantages of these solutions, and any of their features which might be useful to the planned solution.
- Provides a summary of any links with or integrations to existing systems used by the client or any other integration requirements demanded by the client.

# 2 – Behavioural Requirements (40%)

# 2\.1 - How the specifications were arrived at (10%)

- Describes activities taken place during the project intended to elicit requirements
  Clearly shows how efforts to understand the client’s requirements influenced the design of the specifications
- Describes the process of gathering and validating User Stories, citing any relevant approaches or methodologies
  Describes the process by which User Stories informed the preparation of Gherkin features, citing any relevant approaches or methodologies
  Accounts for any difficulties encountered in preparing the User Stories or the Gherkin pseudocode listing and how they were overcome

# 2\.2 – Behavioural specifications (30%)

- Provides a set of behavioural requirements that cover the core functionality of the solution
- Correctly implements the User Story approach to capture client requirements
- Faithfully models features and scenarios that are relevant to the User Story using valid Gherkin pseudocode
- Clearly links each feature to the overall objectives of the client and prioritises appropriately

# 3 – Project Development (30%)

# 3\.1 – Risks and Issues (10%)

- Describes a clearly and carefully considered risk assessment.
- Provides a clear and plausible prioritisation of risks identified.
- Offers a plausible and relevant mitigation strategy for the risks identified.

# 3\.2 – Development approach (10%)

- Justifies which software development approach the group will use.

# 3\.3 – Project Schedule (10%)

- Provides a project schedule (Gantt chart, or something comparable) that identifies key milestones in the project.

# 4 – Writing Skills and Presentation of Document (10%)

The following attributes apply to the whole document:

- Includes an appropriate cover page.
- Conforms to the page limits for each section.
- Is consistent in tone, style, and formatting, rather than reading like a group of people have written individual sections (e.g. standard use of terminology, font type and size.)
- Deploys correct spelling, punctuation, and grammar.
