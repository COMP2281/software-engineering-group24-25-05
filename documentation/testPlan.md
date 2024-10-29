# Project Test Plan

The deliverable for this element of the project is a report that describes the scope of your testing activities, together with a summary report of testing outcomes. Note that your report should use a vocabulary that is consistent with the one provided in the lectures.

## What you should test

Your report should cover the following forms of testing: _unit_ testing, _system_ testing and _user acceptance_ testing (UAT). If the architecture of your application involves a substantial element of module integration, then you should provide a discussion of how you are managing the process of integration testing.

## What should be tested

The first section of your report should identify the main items being tested for both unit and system testing, and the test oracles that will be used for each one. This can be provided as two lists, and for system testing you should indicate whether these are being tested against functional or non-functional requirements (or both).

## Test Cases

The second section of your report should provide a minimum of four (4) and a maximum of six (6) sample test cases for each of the forms of testing being used (including integration testing where appropriate). These should be presented in a tabular form, and an example of such a form being used for a test case for the software in a bank ATM is presented below. You can adapt this table as necessary.

| Test Case ID                         | sys_test-02                                                                                                      |
| :----------------------------------- | :--------------------------------------------------------------------------------------------------------------- |
| Description of test                  | Application successfully reads a bank card                                                                       |
| Related requirement document details | <user story 1>                                                                                                   |
| Pre-requisites for test              | <p>System is operational and not currently servicing a request.</p><p>Bank card is available to be inserted.</p> |
| Test procedure                       | <p>1\. Insert readable card</p><p>2\. System responds with accept or reject message</p>                          |
| Test material used                   | Bank card                                                                                                        |
| Expected result (test oracle)        | Card is accepted and system asks for PIN to be typed in                                                          |
| Comments                             | None                                                                                                             |
| Created by                           | SJ                                                                                                               |
| Test environment(s)                  | Windows 10                                                                                                       |

For the example above, there would need to be a similar test performed with an invalid card.

For each such plan there should be a separate table of the results, and whether or not the test was passed. If a test fails, you should note whether or not the failure is of high or low severity. Where regression testing has been performed, you should provide the test results for each test cycle, indicating whether or not it succeeded for each testing iteration.

Test oracles for system and unit testing should normally be provided by using the requirements and design specifications. For UAT, they will normally be provided directly by the end-users, undertaking the task of applying user stories.

## Testing context

This part of your report should indicate whether or not the tests need to use any specific tools or environment. For example, if a test requires use of a web browser, you should indicate which ones are being used (and why). The same applies to operating systems.

You may want to use a simple scale of test failure severity to distinguish between faults that need to be urgently addressed (the system crashes or generates wrong outcomes) and those that are inconvenient. (See the examples in the first lecture on testing.)

## Writing good test cases

Test cases need to be as atomic as possible, so that it is clear what is being tested and what the result should be. As in the example above, test cases should cover both successful use (with a valid bank card) and unsuccessful use (with an invalid bank card) wherever this is appropriate.

For unit testing, you may want to identify appropriate equivalence classes to help determine how many tests are needed for each unit.

A test case should be expressed clearly and unambiguously using active voice where possible to emphasise what should be done. Any references to other parts of an application should be clearly referenced. You should also make clear what form of test oracle is used.

# Mark Scheme

The general mark scheme that will be used for this deliverable is as follows.

| **_Section_** |                                                                                                                                                                                                                                                                         **_What is expected_**                                                                                                                                                                                                                                                                         | **_Mark_** |
| :-----------: | :--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------: | :--------: |
|       1       |                                                                                                                                                                                          This should identify _all_ of the items that should be covered by the testing phase of the project, the nature of the testing required for each one, and the test oracle to be used.                                                                                                                                                                                          |    20%     |
|       2       |                                                                                                   There should be _at least_ a minimum of 12 test cases presented (but no more than a _maximum_ of 18) along with any outcomes. The choice of test cases should be suitably representative, and marks will be deducted for unnecessary repetition or inadequate coverage. Where appropriate, identify equivalence classes that should be used for a particular test.                                                                                                   |    50%     |
|       3       |                                                                                                                                                                            The discussion of text context, and the choice of classes of failure severity should be appropriate to the nature and form of your application, and where appropriate, you should give reasons for your choices.                                                                                                                                                                            |    20%     |
|       4       | Being professional in your submitted work is expected, use clear formatting like bold text, headings, and bullet points to make your document easy to read by the marker. Start with a table of contents so the reader can easily navigate your work and know what is expected. Provide a short summary of the project at the beginning to explain its purpose and scope. Include a summary of what will be presented in the testing report, outlining the key findings and results. Finally, be concise and organized to demonstrate your professionalism throughout. |    10%     |
