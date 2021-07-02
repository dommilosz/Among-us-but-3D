using Photon.Pun;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Random = UnityEngine.Random;

public class TaskManager : MonoBehaviour
{

    [Serializable]
    public enum TaskTypes
    {
        Common, Short, Long, LongSingle
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    [Serializable]
    public class Task
    {
        public string name = "";
        public GameObject prefab;
        public TaskTypes TaskType = TaskTypes.Short;
        public bool Done = false;

        public bool Active
        {
            get
            {
                var playerInfo = PlayerInfo.getPlayerInfo();
                return playerInfo.Tasks.IsActive(this);
            }
        }

        public static Task GetByName(string name)
        {
            var Tasks = PlayerInfo.getPlayerInfo().Tasks.FullTaskList.Clone();
            Tasks.Shuffle();
            foreach (var item in Tasks)
            {
                if (item.name == name) return item;
            }
            return null;
        }

        public static Task GetByName(string name,TaskTypes type)
        {
            var Tasks = PlayerInfo.getPlayerInfo().Tasks.FullTaskList.Clone();
            Tasks.Shuffle();
            foreach (var item in Tasks)
            {
                if (item.name == name&&item.TaskType==type) return item;
            }
            return null;
        }

        public static void RegisterTasks()
        {
            var playerInfo = PlayerInfo.getPlayerInfo();
            playerInfo.Tasks._FullTaskList.Clear();
            foreach (var item in FindObjectsOfType<TaskObj>())
            {
                playerInfo.Tasks._FullTaskList.Add(item.GetComponent<TaskObj>().task);
            }
        }

        public static Task[] GetAll(TaskTypes type)
        {
            var Tasks = PlayerInfo.getPlayerInfo().Tasks.FullTaskList;
            List<Task> tasks = new List<Task>();
            foreach (var item in Tasks)
            {
                if (item.TaskType == type) tasks.Add(item);
            }
            return tasks.ToArray();
        }
        public static Task[] GetAll()
        {
            List<Task> Tasks = PlayerInfo.getPlayerInfo().Tasks.FullTaskList;
            return Tasks.ToArray();
        }

        public static Task[] GenerateTasks(int count)
        {
            var tasks = GetAll(TaskTypes.Short).ToList();
            tasks.Shuffle();
            var tasksFinal = new List<Task>();
            for (int i = 0; i < count; i++)
            {
                if (i >= tasks.Count) break;
                tasksFinal.Add(tasks[i]);
            }
            return tasksFinal.ToArray();
        }
        public static string[] GenerateCommonTasks(int count)
        {
            var tasks = GetAll(TaskTypes.Common).ToList();
            tasks.Shuffle();
            var tasksFinal = new List<string>();
            for (int i = 0; i < count; i++)
            {
                if (i >= tasks.Count) break;
                tasksFinal.Add(tasks[i].name);
            }
            return tasksFinal.ToArray();
        }

        public bool IsCompleted(string name)
        {
            return GetByName(name).Done;
        }

        public LongTask GetLongTask()
        {
            foreach (var item in PlayerInfo.getPlayerInfo().Tasks.LongTasks)
            {
                if (item.tasks.Contains(this)) return item;
            }
            return null;
        }
    }

    [Serializable]
    public class LongTask
    {
        public List<Task> tasks = new List<Task>();
        public bool Done
        {
            get
            {
                var progress = GetProgress();
                return progress[0] >= progress[1];
            }
        }
        public bool Started
        {
            get
            {
                var progress = GetProgress();
                return progress[0] > 0;
            }
        }

        public static List<string[]> Pairs = new List<string[]>();
        public static List<LongTask> PossibleLongTasks = new List<LongTask>();

        public LongTask(Task[] tasks)
        {
            this.tasks = tasks.ToList();
        }

        public LongTask(Task task)
        {
            tasks.Add(task);
        }

        public static void InitPairs()
        {
            Pairs.Clear();
            PossibleLongTasks.Clear();
            Pairs.Add(new string[] { "download", "upload" });

            foreach (var item in Pairs)
            {
                var tmpTasks = new List<Task>();
                foreach (var item2 in item)
                {
                    tmpTasks.Add(Task.GetByName(item2));
                }
                if(!tmpTasks.Contains(null))
                PossibleLongTasks.Add(new LongTask(tmpTasks.ToArray()));
            }
            //Pairs.Add(new string[] {"wires","wires","wires","wires" });


            foreach (var item in Task.GetAll(TaskTypes.LongSingle))
            {
                LongTask.PossibleLongTasks.Add(new LongTask(item));
            }
        }

        public static LongTask[] GenerateTasks(int count)
        {
            var plt = PossibleLongTasks.Clone();
            plt.Shuffle();
            var tasksFinal = new List<LongTask>();
            for (int i = 0; i < count; i++)
            {
                if (i >= plt.Count) break;
                tasksFinal.Add(plt[i]);
            }
            return tasksFinal.ToArray();
        }

        public Task GetCurrentTask()
        {
            foreach (var item in tasks)
            {
                if (!item.Done)
                {
                    return item;
                }
            }
            return tasks.Last();
        }

        public int[] GetProgress()
        {
            int done = tasks.IndexOf(GetCurrentTask());
            if (GetCurrentTask().Done) done++;
            return new int[] { done, tasks.Count };
        }

        public string GetStringProgress()
        {
            var progress = GetProgress();
            if (progress[1] <= 1)
            {
                return "";
            }
            return $" {progress[0]}/{progress[1]}";
        }
    }

    public class AllTasks
    {
        public List<TaskManager.Task> CompletedTasks = new List<TaskManager.Task>();

        public List<TaskManager.Task> ShortTasks = new List<TaskManager.Task>();
        public List<TaskManager.LongTask> LongTasks = new List<TaskManager.LongTask>();
        public List<TaskManager.Task> CommonTasks = new List<TaskManager.Task>();

        [SerializeField]
        public List<Task> _FullTaskList = new List<Task>();
        public List<Task> FullTaskList
        {
            get
            {
                if (_FullTaskList.Count <= 0)
                {
                    Task.RegisterTasks();
                }
                return _FullTaskList;
            }
        }

        public AllTasks()
        {
            Reset();
        }

        public void Reset()
        {
            CompletedTasks.Clear();
            ShortTasks.Clear();
            LongTasks.Clear();
            CommonTasks.Clear();
        }

        public void GenerateTasks()
        {
            Reset();
            Task.RegisterTasks();
            LongTask.InitPairs();
            ShortTasks = TaskManager.Task.GenerateTasks((int)SettingsHandler.getSetting("Short_Tasks")).ToList();
            LongTasks = TaskManager.LongTask.GenerateTasks((int)SettingsHandler.getSetting("Long_Tasks")).ToList();

            if (PlayerInfo.getPlayerInfo().getPUNPlayer().IsMasterClient)
            {
                var CTasks = TaskManager.Task.GenerateCommonTasks((int)SettingsHandler.getSetting("Common_Tasks"));

                Stream s = new MemoryStream();
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(s, CTasks);
                s.Flush();

                PlayerActions.GetPhotonView().RPC("AddCommonTasks", RpcTarget.All, s.ReadAllBytes());
                s.Close();
            }
        }

        public int[] GetTasksProgress()
        {
            if (PlayerInfo.getPlayerInfo().IsImpostor()) return new int[] { 0, 0 };
            int completed = 0;
            foreach (var item in LongTasks)
            {
                if (item.Done) completed++;
            }
            foreach (var item in ShortTasks)
            {
                if (item.Done) completed++;
            }
            foreach (var item in CommonTasks)
            {
                if (item.Done) completed++;
            }
            return new int[] { completed, ShortTasks.Count + LongTasks.Count + CommonTasks.Count };
        }

        public bool IsActive(Task task)
        {
            if (CommonTasks.Contains(task)) return true;
            if (ShortTasks.Contains(task)) return true;
            if (task.GetLongTask() != null) return true;
            return false;
        }

        public static void AddCommonTasks(byte[] cTasks)
        {
            var CTasks = new List<string>();
            var Tasks = PlayerInfo.getPlayerInfo().Tasks;
            Stream s = new MemoryStream();
            s.WriteAllBytes(cTasks);
            s.Position = 0;
            IFormatter formatter = new BinaryFormatter();
            CTasks = ((string[])formatter.Deserialize(s)).ToList();
            s.Flush();
            s.Close();
            foreach (var item in CTasks)
            {
                Tasks.CommonTasks.Add(TaskManager.Task.GetByName(item, TaskManager.TaskTypes.Common));
            }
        }

        public bool Done
        {
            get
            {
                var progress = GetTasksProgress();
                return progress[0] >= progress[1];
            }
        }
    }
}

