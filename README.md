# BowlingScoreCalculator
Simple bowling score calculator, using the traditional scoring method specified

## Framework

.NET 6.0

## Run the app locally

Navigate to the project folder at BowlingScoreCalculator\BowlingScoreCalculator.Api.
Run the following command to build and run the app locally: 

dotnet run

Go to http://localhost:5071/swagger/index.html
 
 ## BowlingScoreCalculator.Api examples

 ### 1. Perfect game

 Request:

````
{
  "pinsDowned": [10,10,10,10,10,10,10,10,10,10,10,10]
}
 ````

 Response:
 ````
 {
  "frameProgressScores": ["30", "60", "90", "120", "150", "180", "210", "240", "270", "300"],
  "gameCompleted": true
}
````
 
 ### 2. Gutter game

Request:

````
{
  "pinsDowned": [0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0]
}
````

Response:

````
{
  "frameProgressScores": ["0", "0", "0", "0", "0", "0", "0", "0", "0", "0"],
  "gameCompleted": true
}
````
 
### 3. Unfinished game

Request:

````
{
  "pinsDowned": [1,1,1,1,9,1,2,8,9,1,10,10]
}
````

Response:

````
{
  "frameProgressScores": ["2", "4", "16", "35", "55", "*", "*", "*"],
  "gameCompleted": false
}
````

### 4. All spares game

Request:
````
{
  "pinsDowned": [9,1,9,1,9,1,9,1,9,1,9,1,9,1,9,1,9,1,9,1,9]
}
````
Response:
````
{
  "frameProgressScores": ["19", "38", "57", "76", "95", "114", "133", "152", "171", "190"],
  "gameCompleted": true
}
````