# Connect-Three: Android Game with a Picnic Theme

Welcome to **Connect Three**, a delightful Android game with a fun picnic theme! 🧺🍓

# Gameplay (Enable Audio!)
[https://github.com/user-attachments/assets/e11c59fe-5603-447e-b71d-5f5937af4ac5](https://github.com/user-attachments/assets/07071672-b4fe-4c3a-9a6e-3ce9ded95765)

## How to Play
1. Move the `.apk` file to your Android phone.
2. Install and run the game.
3. Enjoy the picnic-themed adventure!

---

## Level Design

### Level 1
For horizontal matches, the probability of a new fruit is 60% of the probability of the fruit directly below the matched fruit.
For vertical matches, the probability of the first new fruit will be 40% of the probability of the fruit directly below the matched fruit and the probability of every subsequent fruit will be 60% of the last generated fruit.
If there are no fruits below the matches fruit(s), then a random generation is applied

### Level 2
A different approach was used:
- After a match is made, we reference the fruit directly below the matched area.
- We analyze the neighboring fruits (8, 5, or 3, depending on the position) to calculate the probability as specified.
- This calculated probability applies to the **first generated fruit**. 
- All subsequent fruits generated will have a **60% chance of matching the color** of the fruit generated immediately before them.

---

## Scoring System

The scoring system was **fine-tuned through playtesting** by me and a friend:
- We played each level multiple times and calculated the average score.
- **Scoring thresholds**:
  - Below the average score: **1 star**
  - At the average score: **2 stars**
  - Above the average score: **3 stars**
- Each fruit match grants **100 points**:
  - A match of five fruits yields **500 points**.
  - A match of six fruits yields **600 points**.

---

Enjoy the game and happy matching! 🎮🍉🍊
