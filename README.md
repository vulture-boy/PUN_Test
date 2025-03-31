Hello! This is a repository for a PUN tutorial I worked on. 
PUN is short for Proton Unity Network; it is a networking library for Unity. https://www.photonengine.com/PUN

**Video**: https://youtu.be/hfPrEzjHcBc

**Players**: 2, networked

**Genre**: Tennis

**Goal**: Score 3 points on your opponent by sending the ball behind them.

**Controls**: Arrow Keys to move the paddle, Click to 'Boost' the power of the rebound on the paddle.


This project is to demonstrate that I have completed the basic tutorial and can work with its functionality to create a basic tennis project.
It was completed over a week while I was employed fulltime in March 2025.

You can find all assets in the Assets folder, including a Windows Build in the Assets/Build folder and a video recording in the Assets/Screenshots folder.
The Git commit history follows the timeline of the tutorial work though the chapters, culminating in the transformation into the final project.

_Some thoughts:_
- The code is not very polished! The goal was to create a proof of understanding of the PUN system within a week, so multiple coding faux pas were committed; very Game Jam core. ;)
- Originally I setup the tennis system to use the physics engine and have the goal to hit the opposing player from behind, sort of like Lethal League. Partway through I realized that it was very likely not going to be easy to keep the game states in synch this way, at least with the amount of time I had to research the system, so I converted the tennis action to always send towards the opponent's back wall when it hit a paddle. I think this strategy sidnificantly reduced the risk involved in shipping the project and the results are good enough, I think!
- Moving forward, I would very much like to read more of the documentation of Proton's other systems of networking, both to take advantage of more powerful, modern systems and generally get to better know how everything functions under the hood.
