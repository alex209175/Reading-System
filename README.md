<h1>Usage while in Unity Editor</h1>

<li>Set the screen resolution in the game view of the Unity Editor to 2048x1536 (the screen resolution of an iPad).</li>
<li>Alternatively, you can also use the Unity remote app on an iPad to be able to stream from the Unity Editor to the iPad through a lightning cable.</li>
<li>Open the WelcomeScreen.unity scene file in the Scenes folder, and press play to begin testing.</li>

<h2>Clearing data</h2>

<li>If you would like to clear the data, in order to change from a teacher to a student, open WelcomeScreen.cs, and in void Start(), add the line PlayerPrefs.DeleteAll();</li>
<li>Run WelcomeScreen.cs once, and this will clear all PlayerPrefs data.</li>
<li>Then, in Firebase, open the Authentication page, and delete all of the accounts.</li>
<li>Under "Build" on the left hand side of the page, click on Realtime Database, and click the "delete" button next to where it says "classes".</li>
