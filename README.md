Connect three android game with a picnic theme
Please move the .apk file to your android phone and install and run the game.
Enjoy!

Level Design
For Level 1, the probability mechanics were implemented exactly as specified.
For Level 2, I took a different approach: After a match is made, we reference the fruit directly below the matched area. Then, we analyze the neighboring fruits (8, 5, or 3, depending on the position) to calculate the probability as specified. This probability applies to the first generated fruit. All subsequent fruits generated will have a 60% chance of matching the color of the fruit generated immediately before them.
Scoring
The scoring system was fine-tuned through playtesting by me and a friend. We played each level multiple times, calculated the average score, and set this as the two-star threshold. Scores below this threshold receive one star, while higher scores receive three stars. Each fruit match grants 100 points, so a match of five or six fruits would yield 500 or 600 points, respectively.

