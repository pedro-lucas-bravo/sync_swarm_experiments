# Interactive Sonification of 3D Swarmalators

This repository contains a [Unity](https://unity.com/)  and a [Max](https://cycling74.com/products/max) project to replicate the work from the paper [**Interactive Sonification of 3D Swarmalators**](https://www.duo.uio.no/handle/10852/111717) presented at [NIME 2024](https://www.nime2024.org/). The Swarmalators model was introduced in the paper [**Oscillators that sync and swarm**](https://www.nature.com/articles/s41467-017-01190-3). A video of a performance showing the capabilities of this system is shown here https://youtu.be/rgXaFE6npD8. You can check the paper to go deeper into the rationale behind this work.

## Running the system

This project works on two different environments (Unity and Max) communicated through [OSC](https://ccrma.stanford.edu/groups/osc/index.html) messages. It has been tested on a Windows machine but should also work on a Mac. Follow the next steps:

1. Open the Unity project and then the scene [`OriginalSwarmalators.unity`](https://github.com/pedro-lucas-bravo/sync_swarm_experiments/blob/main/Assets/Scenes/OriginalSwarmalators.unity). Play the scene and 3 agents will be instantiated.

2. For real-time control, you need the Max project. Open the project [`audio_swarmalators.maxproj`](https://github.com/pedro-lucas-bravo/sync_swarm_experiments/blob/main/max/projects/audio_swarmalators/audio_swarmalators.maxproj). The main file is called [`MainController.maxpat`](https://github.com/pedro-lucas-bravo/sync_swarm_experiments/blob/main/max/projects/audio_swarmalators/patchers/MainController.maxpat) where you can control the system.

3. Press the red button `/connect 127.0.0.1 2001` and you should be able to listen the output of the 3 agents instantiated in the Unity project.

4. You can manipulate controls directly in Max, but also you have the possibility to use a MIDI controller; for now, two controllers has been used, an **AKA mini II** and an **Arturia Minilab**. You can implement your own mapper, as we have for those two controllers, to use your own.

5. Explore the project. We have a mapping for frequencies and another for rhythms which can be used at the same time.


## The Unity project

The Unity project is developed using the version [2022.3.8f1](https://unity.com/releases/editor/whats-new/2022.3.8). The **Swarmalators model** is implemented in the script [`Swarmalator.cs`](https://github.com/pedro-lucas-bravo/sync_swarm_experiments/blob/main/Assets/Scripts/Swarmalator.cs) and the **Interactive Swarmalator** in [`ManualSwarmalator.cs`](https://github.com/pedro-lucas-bravo/sync_swarm_experiments/blob/main/Assets/Scripts/ManualSwarmalator.cs). The agents are managed centrally through the script [`MainSyncSwarm.cs`](https://github.com/pedro-lucas-bravo/sync_swarm_experiments/blob/main/Assets/Scripts/MainSyncSwarm.cs). For OSC communication, the script [`ExternalCommunicationManager.cs`](https://github.com/pedro-lucas-bravo/sync_swarm_experiments/blob/main/Assets/Scripts/External%20Communication/ExternalCommunicationManager.cs) defines the messages and implements the management of the agents in connection with an external system (in this case the Max project). You would need a fair understanding of the Unity game engine to explore the project.

##  The Max Project

The Max project was developed using the version 8.5.6. The main file is [`MainController.maxpat`](https://github.com/pedro-lucas-bravo/sync_swarm_experiments/blob/main/max/projects/audio_swarmalators/patchers/MainController.maxpat). To explore this project we recommend to look at topics related to OSC messages (UDP connections), [`mc`](https://docs.cycling74.com/max8/vignettes/mc_topic) objects, especially the [`mc.poly~`](https://docs.cycling74.com/max8/refpages/mc.poly~) object to manage multiple audio signals, and the fundamentals for using [`Max`](https://cycling74.com/products/max).

## Data Analysis

There are modules in the main Max patch to record sound data and we have the data analysis code in a Jupiter notebook [here](https://github.com/pedro-lucas-bravo/sync_swarm_experiments/blob/main/data_analysis/swarmalators_scalability.ipynb).

## License

This software is released under the [GNU General Public License 3.0 license](https://www.gnu.org/licenses/gpl-3.0.en.html).


