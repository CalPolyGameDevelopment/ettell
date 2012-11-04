
Wave Defence 

- Wave defence is characterized by "waves" of enemies attempting to
  kill/destroy/otherwise compromise the player.

  - Waves are characterized by the following

    - During a wave objects spawn

      - Objects can be:

        - buffs
        - debuffs
        - enemies
        - friends
	- etc.
 
    - Often they do not spawn all at once and the wave and they are
      staggered by some sort of trigger:

      - killing enemies
      - time
      - player status 
      - etc.

    - These functional "spawn" portions of the waves I've decided to
      label as "phases"

- From the above, it is easy to see that waves can be abstracted to
  state a state machine.

- Wave Example0: The base case would be spawning a single enemy.

  - Phase 0: Game Begin

  - Phase 1: Enemey alive

  - Phase 1: Wave end
    - Trigger: Enemy killed

- Wave Example2: Spawning 1 enemey and spawning another after the first
  one's death

   - Wave init Phase 0 ->