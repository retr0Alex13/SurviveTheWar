namespace OM
{
    public class GatheringGoal : Task.TaskGoal
    {
        public string item;

        public override string GetDescription()
        {
            return $"Gather a {item}";
        }

        public override void Initialize()
        {
            base.Initialize();
            EventManager.Instance.AddListener<GatheringGameEvent>(OnGathering);
        }

        private void OnGathering(GatheringGameEvent eventInfo)
        {
            if (eventInfo.itemName == item)
            {
                CurrentAmount++;
                Evaluate();
            }
        }
    }
}