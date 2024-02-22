# scoreboard
scoreboard test assignment

# requirements
The scoreboard supports the following operations: 
1. Start a new match, assuming initial score 0 – 0 and adding it the scoreboard. 
This should capture following parameters:
a. Home team
b. Away team
2. Update score. This should receive a pair of absolute scores: home team score and away 
team score. 
3. Finish match currently in progress. This removes a match from the scoreboard.
4. Get a summary of matches in progress ordered by their total score. The matches with the 
same total score will be returned ordered by the most recently started match in the 
scoreboard. 

# models
LiveMatch - main model of the project that represents a blueprint of the live match with <br>
the following properties: <br>
Id - unique identifier of the live match.<br>
HomeTeam - the home team. Simplified to a string.<br>
AwayTeam - the away team. Simplified to a string.<br>
Score - score for the current match, simplified to a tuple.<br>
StartedDateTime - the timestamp when the match has started.<br>
<br>
CreateLiveMatchValidator - a validation class of FluentValidation AbstractValidator<br>
that will check all required fields for match creation for validity.<br>
<br>
UpdateLiveMatchValidator - adopts validation rules from CreateLiveMatchValidator <br>
and checks the identifier of the match.<br>
<br>
ILiveMatchRepository - an abstraction for interaction with live matches<br>
data source. Contains the following methods:<br>
GetAll - gets all currently live matches.<br>
Insert - inserts into data source new live match, returns an id of inserted live match.<br>
Update - updates the live match.<br>
Delete - deletes the live match by its id.<br>
<i>Later I thought that extending it with GetById would be a good enhancement</i><br>
<br>
InMemoryLiveMatchRepository - a sample repository implementation for the collection of live matches.<br>
<br>
LiveScoreboard - represents a live scoreboard with the following methods: <br>
StartMatch - starts a new match <br>
UpdateMatchScore - updates a new match <br>
FinishMatch - finishes a new match <br>
GetSummary - returns ordered live match list. <br>
<br>
# author comments
<i>
This is the first time I had to work with a TDD approach. 
Before coding a test task, I studied the subject area and tried to 
write a small class for myself in TDD.</i>
<br>
<i>
I chose MSTest and Moq for writing tests because this is the framework with which I have worked the most. 
However, I know that NUnit and Fluent Assertions would also be good.
</i>
<br>
<i>
In total, together with preliminary preparation and writing this code, I spent ~ 8 hours.
The experience was very interesting and at first it was difficult 
to twist my brain to write a test using methods that do not yet exist</i>