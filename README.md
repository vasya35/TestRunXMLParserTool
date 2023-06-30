<p align="center">
   <img src="/assets/logo/Logo 1000x1000.png" alt="TestRunXMLParserTool" width="200" align="center" >
</p>

# Introduction
<p>
The TestRunXMLParserTool views xml file with testrun results and helps select test cases (using filters or/and sorting) for generate xml suit for rerun or generate js script for select test cases in testrail in <b>three simple steps</b>.
</p>
<p>
The TestRunXMLParserTool is Windows desktop app written in C# using <code>WPF</code>, <code>MVVM</code> and <code>ReactiveUI</code>.
</p>

## Step 1 - Open file
 <p align="center">
   <img src="/assets/screenshots/Step1.png" alt="Step1 screenshot" width="650" align="center" >
</p>
<p>
Open file dialog window will be open immediately after run application.
</p>

## Step 2 - Filter and Sort 
 <p align="center">
   <img src="/assets/screenshots/Step2.png" alt="Step2 screenshot" width="650" align="center" >
</p>
<p>
You can select test case results by them status (PASS, FAIL, SKIP) and enable/disable SORT<b>*</b> by number (without prefix letter) them.
</p>
<p>
<b>*</b> - If SORT isn't selected, test cases will be sort such as original xml file. 
</p>

## Step 3 - Generate file
 <p align="center">
   <img src="/assets/screenshots/Step3.png" alt="Step3 screenshot" width="650" align="center" >
</p>

- Generate XML for rerun - generated XML can be inserted to IDE for rerun.
- Generate JS for selected testcases in testrail - script also will be copy to clipboard after save file.

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

<p align="center"><b><i>Change language will be applied after restart application.</i></b></p>
  
<p>
Listener name will be included in XML for rerun. 
</p>

<br>

### XMLResultGenerator

<p>This project is used to generate original XML files with random test case resuls.</p>
<p>It's console application. After run you need input count of test cases for generate and press enter.</p>

