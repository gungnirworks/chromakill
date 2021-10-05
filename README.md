# chromakill
Chromakill Mechanics Rebuild 00: This repository is scripts only.

This is a ground-up rebuild of my Chromakill project to serve as a more solid foundation on top of which to build an action game.

20-10-05:
Input buffer demonstration can be found here: https://www.youtube.com/watch?v=aMKVakpckkQ .
Polling for user input in Update() is more responsive, and having game logic act on FixedUpdate() timing is more reliable. On a more elementary level, this solution also means I'm always only checking the input buffer logged in one script, instead of polling for user input in numerous places. There are various other potential benefits of logging user button inputs.
