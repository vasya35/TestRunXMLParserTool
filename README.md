<p align="center">
   <img src="/assets/logo/Logo 1000x1000.png" alt="TestRunXMLParserTool" width="200" align="center" >
</p>

# Introduction
<p>
The TestRunXMLParserTool parsers xml file with testrun results and helps select test cases (using filters or/and sorting) to generate xml suit for rerunning or generate js script to check test cases in testrail in <b>three simple steps</b>.
</p>
<p>
The TestRunXMLParserTool is Windows desktop app written in C# using <code>WPF</code>, <code>MVVM</code> and <code>ReactiveUI</code>.
</p>

## Step 1 - Open file
 <p align="center">
   <img src="/assets/screenshots/Step1.png" alt="Step1 screenshot" width="650" align="center" >
</p>
<p>
Open file dialog window will be open immediately after launching the application.
</p>

## Step 2 - Filter and Sort 
 <p align="center">
   <img src="/assets/screenshots/Step2.png" alt="Step2 screenshot" width="650" align="center" >
</p>
<p>
You can select test case results by their status (PASS, FAIL, SKIP) and enable/disable SORT<b>*</b> by test case number without prefix letter.
</p>
<p>
<b>*</b> - If SORT isn't selected, test cases will be sort such as original xml file. 
</p>

## Step 3 - Generate file
 <p align="center">
   <img src="/assets/screenshots/Step3.png" alt="Step3 screenshot" width="650" align="center" >
</p>

- Generate XML for reruning - generated XML can be inserted to IDE for reruning.
- Generate JS for selected testcases in testrail - script also will be copied to clipboard after saving the file.

## Settings window
 <p align="center">
   <img src="/assets/screenshots/Settings.png" alt="Step3 screenshot" width="650" align="center" >
</p>
<p>
Settings window contains two options: select language and input listener name.
</p>
<p>
  
Supported languages: 
</p>

- English
- Russian
- Kazakh
- Turkish

<p align="center"><b><i>Change language will be applied after the application has been restarted.</i></b></p>
  
<p>
Listener name will be included in XML for rerunning. 
</p>

<br>

### XMLResultGenerator

<p>This project is used to generate original XML files with random test case results.</p>
<p>It's a console application. After launching the application you need to input the number of test cases for generating and press enter.</p>

