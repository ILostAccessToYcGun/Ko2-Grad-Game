<img width="315" height="250" alt="image" src="https://github.com/user-attachments/assets/86662521-c797-4050-87bd-8e2bce0d4ef9" />

# Med School Survivors
A vampire survivors-like where you kill enemies, level up and evolve your weapons. This project was made in 8 days where I created 100% of the code and my sister created 100% of the visual assets without the use of AI. This project was created as a graduation present for my brother who graduated from his 6-year medical degree.

# Technology
This project is a **Unity 2D project**. It utilizes the full extent of Unity, from code creation to asset modification and animations.
Additionally external drawing software was used for the visual assets.

# Features
Med School Survivors adopts many of the classic features of vampire survivor-likes.
- A vareity of weapons to use
- Ramping enemy difficulty
- A final boss
- Randomized upgrades for a roguelike gameplay loop

In addition, I also created a **unique weapon evolution system**.
- Each weapon has a primary and secondary attack
- Killing enemies with the base weapons will accumulate kills/points towards an evolution depending on the attack used
  - E.g Killing enemies with the primary slash attack of the Unforged Weapon will contribute to the evolution of the Scout Blades, but not the Cyclic Flail
- This means with 3 starting weapons, 2 evolutions per weapon, and 2 attacks per weapon, there are 18 unique attacks in the game.

# Development Process
This project had a deadline of **10 days**. It would have been longer if I wasn't focusing on my university assignmments.

I had to properly plan out my tasks and communicate with my sister to efficiently complete the game on time.

First I spent an entire day on ideation, documentation and planning. I didn't even know what kind of game I was going to make until my time was ticking so I needed things to be set in stone. 
The 3 pillars I focused on during documentation was Setting, Scope and Mechanics which allowed me to outline the game without getting caught in the fine details. This handled both the ideation and the documentation which I could then share with my sister.
Then I planned out my schedule, which ended up going off the rails anyway because of overestimations. 

It looked something like this to begin with:

<img width="573" height="323" alt="image" src="https://github.com/user-attachments/assets/8c160fed-80b5-4744-b508-ea18463fff8b" />

But I needed to spend more time on the weapons, because 18 attacks is a lot.

After that it was just a matter of working through that schedule and putting in the work.
On most days I created a TODO: list for things that I wanted completed on that day which helped to keep me on track and acted as a pseudo standup meeting for the Agile Development cycles.

E.g

<img width="567" height="600" alt="image" src="https://github.com/user-attachments/assets/176ba058-6abc-414e-98f0-5355a0f861cd" />

(Note: P = Particles, C = Camera effects/movement, T = Trails)
Because particles, cameras and trails is the golden trifecta of juice and player feedback. (In hindsight, sound should go there aswell)

# Lessons/Reflection
This project was less about learning and more of an application of all I have learned. I applied my prior knowledge from my own works and from my university assignments to assist in the creation of the game. One example of this is creating static singletons, simple but very effective. Additionally I learned about sprite slicing, this allowed me to adjust UI sprites for effective and aesthetic scaling on UI. 

I think this project was a good experience. It showed me that I can lead a project from ideation through development and into delivery. it also revealed some weakpoints, such as oversetimating my scheduling.

# Improvements
The project is by no means perfect. From a programming perspective, many of the systems in place, are not optimized the best. Due to the short time constraint I focused on making things functional over making them as efficient as possible, however I made optimizations where I could. For example I used many base classes and a parenting to make the code as dry as possible, however some systems like the weapon attack speed system are not as elegant.

# Future Plans
I currently have no plans to continue content development in the game, as this was a one time present and not a live ops project. However in the future I may look to replace the music in the game to remove copywrite restraints. I find this change quite minute and insignificant, so I don't have any urgency to make this change.


