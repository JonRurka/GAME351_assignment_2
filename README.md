"# GAME351_assignment_2" 


We first set out to implement a basic movement script to travel along the ground,
set the height of the vehicle based on the heightmap height at the crafts point, and 
applying a sin wave to make the craft's height vary slightly when still. We then set
out to apply some physics based movement along with the craft changing rotation based
the terrain below it as to not clip the terrain. 

Multiple different methods were attempted to use colliders and rigidbodies to drive movement
accross the terrain in a realistic fashion. Among the failed ideas, promising runner-ups
included simply placing colliders under the vehicle with no constraints on position or
rotation for the entire geometry as an whole unit. This was mostly stable, but could 
result in the craft flipping over. To counter the craft flipping over using this method,
we attempted to use ApplyForceAtPosition on the rigidbody to apply forces at the corners 
of the craft based on the respective corners distance from the ground, which resulted
in instabilities in the physics system calculating moments and leading to very unstable
behavior.

Ultimately, We opted to make the rotation of the craft as it goes over hills not driven by
the physics system, and used the rigidbody to determine position only, with the rotation 
being set via custom calculations using normal vectors determined by the heightmap. Colliders
were placed under the vehicle to rest on, with the overall rigidbody having contraints on each
axis of rotation and movement being free, with the craft resting on the colliders. We sampled
the terrain height in the middle of the vehicle, and some point near the front of the vehicle
and created a local forward vector tangent to the slope of the terrain beneath the craft. We 
translated this forward vector to world space in relation to the parent rigidbody, and set the
craft's model to use this forward vector, so the visual model could point up or down without 
affecting the orientation of the parent rigidbody and colliders. We then utilized the local 
forward vector in the forward rigidbody translation calls triggered with the "D" and "S" keys
to smoothly move the rig tangentally along the terrain surface. Previously, just moving directly
forward with the keypress would cause clipping with the terrain, and using this local forward
vector calculated using the terrain heightmap proved to be a very satisfying fix.

After creating the movement script, we duplicated the structure of the first craft to the other 
two provided models to create three prefabs, with varying speed and cornering for each. We created
a spawning script to iterate to the next prefab in an array of crafts, and cycle back to the first
craft at the end, destroying the previous craft and restarting the position to spawn. We finally created
a laser prefab, a scipt for the laser beam that simply moves it forward and destroys it if it goes
further than 1000 meters, and instantiated the laser from the movement script if space is pressed.

One complication that currently exists that we haven't quite solved yet is implementing a side-tilt 
rotation along with the forward tilting motion while driving over terrain. We attempted to create
a right vector based off the terrain height similarly to the forward vector, however setting the
model.right vector immediatly after setting the model.forward vector causes instability and jittery
motion in the craft's model. There might be ways to solve this with a modified lookAt method that uses
a forward and right vector instead of just a forward vector, and is something to look into.

