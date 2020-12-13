namespace Gsemac.Text {

    public delegate string CharReplacementEvaluatorDelegate(char inputChar);

    public interface ICharReplacementEvaluator {

        string GetReplacement(char inputChar);

    }

}