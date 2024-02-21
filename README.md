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
LiveMatch - main model of the project that represents a blueprint of the live match with 
the following properties: 
Id - unique identifier of the live match.
HomeTeam - the home team. Simplified to a string.
AwayTeam - the away team. Simplified to a string.
Score - score for the current match, simplified to a tuple.
StartedDateTime - the timestamp when the match has started.

InMemoryLiveMatchRepository - a collection of live matches.

