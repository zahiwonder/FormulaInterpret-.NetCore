# FormulaInterpret-Java  
**Interpret a plain text formula, pass variables, then get result.  
So you can store formula in database or file, and pass variables during runtime.**  

**Rules:**  
Available operators:+ - \* /  
Parentheses are also available.  
All blank(if exists) will be ignored.  

**Run efficiency:**  
Use a four variables and two constants formula to test, run on I7 6600U.  
Construct costs 6.6s for one million times.  
Pass Variables and get float result costs 3.4s for one million times.  

**Sample Code:**  
Formula formula=new Formula("Var1+(Var2+1)\*5"); //instantialize a formula object  
HashMap<String,String> variableCollection=new HashMap<String,String>(); //create a variable name-value Map  
variableCollection.put("Var1","10");  
variableCollection.put("Var2","20");  
Float resultFloat=formula.resultAsFloat(variableCollection);  //get float result  
Integer resultInteger=formula.resultAsInteger(variableCollection);  //get integer result  
