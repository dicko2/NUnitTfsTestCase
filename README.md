# NUnitTfsTestCase

A library used to read and write to TFS Test Case Work Items from NUnit unit tests.

Currently it only supports scenario where you are using on premise TFS server and your build agent is running under a windows account with the appropriate access to the TFS server. Built using the Old TFS client library not the new REST library, i will update it to the REST library in future when i get it working with VSTS (Formally known as VSO).

To use this you will need to add to appSettings to your project for the location of your TFS server and the project name
```xml
<add key="tpcUri" value="https://mytfsserver.com.au/tfs/DefaultCollection" />
<add key="teamProjectName" value="MyProjectName" />
```
To use the Test case update you need to add the Test Suite ID and Test Plan ID in your test fixture constructor, and also inherit your test fixture from the ControllerTestRig, example as follows.
```csharp
[TestFixture]
class MyTestClass : ControllerTestRig
{
public MyTestClass () //ctor for setting static vars, runs before OneTimeSetup
{
NUnitTfsTestCase.TfsService.RunData.TestPlanId = 65213;
NUnitTfsTestCase.TfsService.RunData.TestSuiteId = 65275;
}
 
[Test]
[TfsTestCase(65871)]
public void MyUnitTestMethod()
{
// Test stuff here
}
}
```
To read data for NUnit parameters you need to add a class for each method and hard code your test case in in here. Then use NUnits TestCase Data source, as follows:
```csharp
using System.Collections.Generic;
using NUnitTfsTestCase.TestCaseData;
 
namespace MyNamespace.MyFixture.MyMethod // use a good folder structure please 
{
 class GetTestCaseData : TestCaseDataTfs
 {
 public static IEnumerable<dynamic> GetTestData()
 {
 return TestCaseDataTfs.GetTestDataInternal(65079);
 }
 }
}

[TestFixture]
class MyTestClass
{
 
[Test]
[Test, TestCaseSource(typeof(MyFixture.MyMethod.GetTestCaseData), "GetTestData")]
public void MyUnitTestMethod(strin someParam1, string someParam2)
{
// Test stuff here
}
}
```
