public interface IAssistable {

    Hero AsisstantHero { get; set; }

    void AssistableActionStart(BaseAction baseAction);
    void PerformAssistableAction();

}