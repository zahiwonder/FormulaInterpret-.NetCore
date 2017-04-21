using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;

namespace FormulaInterpret
{
    public class Formula
    {
        //
        //private data
        private string formulaInner;
        private Dictionary<string, float> variableCollection;
        private List<Object[]> operationCollection;
        //
        //public data
        public string formula
        {
            get
            {
                return this.formulaInner;
            }
        }
        //
        public Formula(string formula)
        {
            this.formulaInner = formula.Replace(" ", "");
            //
            //turn formula to inner data structure
            this.operationCollection = this.convertToOperationList();
        }


        //
        //output as float
        public float resultAsFloat(Dictionary<string, string> variableCollection)
        {
            string resultString = resultAsString(variableCollection);
            return float.Parse(resultString);
        }

        //
        //output as integer, ignore decimal part
        public int resultAsInteger(Dictionary<string, string> variableCollection)
        {
            string resultString = resultAsString(variableCollection);
            return Convert.ToInt32(resultString.Substring(0, resultString.IndexOf(".")));
        }

        //
        //check the variables and output the result as string
        private string resultAsString(Dictionary<string,string> variableCollection)
        {
            this.variableCollection = new Dictionary<string, float>();
            //
            //check if the variables are integer of float
            foreach(KeyValuePair<string,string> keyValuePairInstance in variableCollection)
            {
                string variableName = keyValuePairInstance.Key;
                string variableValue = keyValuePairInstance.Value;
                if (this.isNumber(variableValue))
                {
                    this.variableCollection.Add(variableName,float.Parse(variableValue));
                }
                else
                {
                    throw new FormatException();
                }
            }
            float resultFloat = calculate(this.operationCollection);
            return resultFloat.ToString();
        }

        //
        //pass variables into formula data structure and return a float
        private float calculate(List<object[]> operationCollection)
        {
            float returnValue = 0f;
            foreach(object[] objectInstance in operationCollection)
            {
                object operatorString = objectInstance[0];
                object operand = objectInstance[1];
                float operandFloat = 0f;
                if(operand is string)
                {
                    string operandString = operand as string;
                    if (variableCollection.ContainsKey(operandString))
                    {
                        operandFloat = variableCollection[operandString];
                    }
                    else
                    {
                        operandFloat = float.Parse(operandString);
                    }
                }
                else if (operand is List<object[]>)
                {
                    operandFloat = calculate(operand as List<object[]>);
                }
                else
                {
                    throw new InvalidCastException();
                }
                switch (operatorString as string)
                {
                    case "+":
                        returnValue += operandFloat;
                        break;
                    case "-":
                        returnValue -= operandFloat;
                        break;
                    case "*":
                        returnValue *= operandFloat;
                        break;
                    case "/":
                        returnValue /= operandFloat;
                        break;
                    default:
                        throw new FormatException();
                }
            }
            return returnValue;
        }

        private List<Object[]> convertToOperationList()
        {
            //
            //match operation
            string operators = "+\\-\\*/";
            string formulaInterpretRegex = "([" + operators + "]{0,1})([^" + operators + "]+)";
            Regex regex = new Regex(formulaInterpretRegex);
            //
            Stack<List<Object[]>> operationListStack = new Stack<List<Object[]>>(); //store operationList
            operationListStack.Push(new List<Object[]>());
            Stack<string> operatorStack = new Stack<string>();  //whenever an element pushes to operationListStack, an operator pushes tp thos stack
            operatorStack.Push("+");

            string currentOperationString = "";
            bool readyToCreateAnOperation = true;
            int loopInt_1 = 0;  //this variable indicate current position while looping
            while (loopInt_1 < this.formula.Length)
            {
                string currentCharacter = this.formula.Substring(loopInt_1, 1);
                if (currentCharacter.Equals("("))
                {
                    if (currentOperationString.Equals(""))
                    {
                        operatorStack.Push("+");
                    }
                    else
                    {
                        operatorStack.Push(currentOperationString);
                        currentOperationString = "";
                        readyToCreateAnOperation = true;
                    }
                    operationListStack.Push(new List<Object[]>());
                }
                else if (currentCharacter.Equals(")"))
                {
                    if (currentOperationString.Equals("") == false)
                    {
                        //
                        //create an operation
                        this.generateOperation(operationListStack.Peek(), currentOperationString, regex);
                        currentOperationString = "";
                        readyToCreateAnOperation = true;
                    }
                    List<object[]> newlyFinishedOperationCollection = operationListStack.Peek();
                    string operatorString= operatorStack.Peek();
                    operationListStack.Pop();
                    operatorStack.Pop();
                    object[] currentOperation = new object[2];
                    currentOperation[0] = operatorString;
                    currentOperation[1] = newlyFinishedOperationCollection;
                    operationListStack.Peek().Add(currentOperation);
                }
                else if (this.isOperator(currentCharacter))
                {
                    if (currentOperationString.Equals(""))
                    {
                        //
                        //this is the beginning of an operation
                        currentOperationString = currentCharacter;
                        readyToCreateAnOperation = false;
                    }
                    else
                    {
                        //
                        //this is the end of an operation
                        generateOperation(operationListStack.Peek(),currentOperationString,regex);
                        //
                        //and another operation's beginning
                        currentOperationString = currentCharacter;
                    }
                }
                else
                {
                    //
                    //variable or number
                    if (readyToCreateAnOperation)
                    {
                        currentOperationString = "+" + currentCharacter;
                        readyToCreateAnOperation = false;
                    }
                    else
                    {
                        currentOperationString = currentCharacter;
                    }
                    //
                    //string ends, create an operation object
                    if (loopInt_1 == this.formula.Length - 1)
                    {
                        generateOperation(operationListStack.Peek(), currentOperationString, regex);
                        currentOperationString = "";
                    }
                }
                this.printOperationList(operationListStack.Peek());
                loopInt_1++;
            }
            return operationListStack.Peek();
        }

        //
        //generate an operation through string
        private void generateOperation(List<Object[]> currentOperationCollection, string currentOperationString, Regex regex)
        {
            Match match = regex.Match(currentOperationString);
            if (match.Success)
            {
                string thisOperator = (match.Groups[1] as Group).Value;
                string thisOperand = (match.Groups[2] as Group).Value;
                if (currentOperationCollection.Count != 0)
                {
                    //
                    //analyse operation priority
                    string lastOperator = currentOperationCollection[currentOperationCollection.Count - 1][0] as string;
                    object lastOperand = currentOperationCollection[currentOperationCollection.Count - 1][1];
                    Console.WriteLine();
                    if (thisOperator.Equals(lastOperator) == false && this.operationLevel(thisOperator) < this.operationLevel(lastOperator))
                    {
                        //
                        //this operation has priority over last operation
                        //
                        //remove last operation from operation list
                        currentOperationCollection.RemoveAt(currentOperationCollection.Count - 1);
                        //
                        //create a new nested operation and add to the operation list
                        object[] lastOperation = new object[2];
                        lastOperation[0] = "+";
                        lastOperation[1] = lastOperand;
                        object[] thisOperation = new object[2];
                        thisOperation[0] = thisOperator;
                        thisOperation[1] = thisOperand;
                        List<object[]> nestedOperationList = new List<object[]>();
                        nestedOperationList.Add(lastOperation);
                        nestedOperationList.Add(thisOperation);
                        object[] nestedOperation = new object[2];
                        nestedOperation[0] = lastOperator;
                        nestedOperation[1] = nestedOperation;
                        currentOperationCollection.Add(nestedOperation);
                    }
                    else
                    {
                        object[] returnObject = new object[2];
                        returnObject[0] = thisOperator;
                        returnObject[1] = thisOperand;
                        currentOperationCollection.Add(returnObject);
                    }
                }
                else
                {
                    object[] returnObject = new object[2];
                    returnObject[0] = thisOperator;
                    returnObject[1] = thisOperand;
                    currentOperationCollection.Add(returnObject);
                }
            }
        }

        //
        //print the operation
        private void printOperation(object[] operation,int level)
        {
            object operatorString = operation[0];
            object operand = operation[1];
            if(operand is string)
            {
                for(int forInt_1 = 0; forInt_1 < level - 1; forInt_1++)
                {
                    Console.WriteLine(" ");
                }
                Console.WriteLine(operand as string + operand as string);
            }else if(operand is List<object[]>){
                foreach(object[] operationInstance in operand as List<object[]>)
                {
                    this.printOperation(operationInstance, level + 1);
                }
            }
            else
            {
                Console.WriteLine(operand.ToString());
            }
        }

        //
        //print the formula data structure
        private void printOperationList(List<object[]> operationCollection)
        {
            foreach(object[] operationInstance in operationCollection)
            {
                printOperation(operationInstance, 1);
            }
        }

        //
        //return priority level of a specific operator
        private int operationLevel(String operatorString)
        {
            if (operatorString.Equals("*") || operatorString.Equals("/"))
            {
                return 2;
            }
            else if (operatorString.Equals("+") || operatorString.Equals("-"))
            {
                return 3;
            }
            else
            {
                throw new FormatException();
            }
        }

        //
        //check if the string is an integer or float
        private bool isNumber(string stringParameter)
        {
            Regex regex = new Regex("[+-]{0,1}[0-9]+\\.*[0-9]*");
            Match match = regex.Match(stringParameter);
            if (match.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //
        //check if the string is an operator
        private bool isOperator(string stringParameter)
        {
            if(stringParameter.Equals("+") || stringParameter.Equals("-") || stringParameter.Equals("*") || stringParameter.Equals("/"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
