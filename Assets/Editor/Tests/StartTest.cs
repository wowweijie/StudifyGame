using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.TestTools;
using UnityEngine.EventSystems;
using UnityEditor.SceneManagement;

namespace Tests
{
    public class StartTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void Login_Validation_Test()
        {
            // Use the Assert class to test conditions
            EditorSceneManager.OpenScene("Assets/FPS/Scenes/LoginMenu.unity");
            InputField usernameField = GameObject.Find("UsernameField").GetComponent<InputField>();
            InputField passwordField = GameObject.Find("PasswordField").GetComponent<InputField>();
            usernameField.text = " ";
            usernameField.text = " ";
            LoginManager loginManager = GameObject.Find("LoginManager").GetComponent<LoginManager>();
            loginManager.OnLogin();
            Assert.IsTrue(GameObject.Find("LoginFailed(Clone)"));
        }

        [Test]
        public void Main_Menu_Render_Button_Test()
        {
            // Use the Assert class to test conditions
            EditorSceneManager.OpenScene("Assets/FPS/Scenes/IntroMenu.unity");
            Assert.IsTrue(GameObject.Find("CustomButton") && GameObject.Find("CampaignButton"));
        }

        [Test]
        public void Main_Menu_Load_Campaign_Test()
        {
            // Use the Assert class to test conditions
            EditorSceneManager.OpenScene("Assets/FPS/Scenes/IntroMenu.unity");
            Button campaignButton = GameObject.Find("CampaignButton").GetComponent<Button>();
            campaignButton.onClick.Invoke();
            Scene scene = EditorSceneManager.GetActiveScene();
            Assert.AreEqual("CampaignMenu", scene.name);
        }

        [Test]
        public void Main_Menu_Load_Custom_Test()
        {
            // Use the Assert class to test conditions
            EditorSceneManager.LoadScene("Assets/FPS/Scenes/IntroMenu.unity");
            Button customButton = GameObject.Find("CustomButton").GetComponent<Button>();
            customButton.onClick.Invoke();
            Scene scene = EditorSceneManager.GetActiveScene();
            Assert.AreEqual("CustomMenu", scene.name);
        }

        [Test]
        public void Campaign_Menu_Load_Categories_Test()
        {
            // Use the Assert class to test conditions
            EditorSceneManager.OpenScene("Assets/FPS/Scenes/CampaignMenu.unity");
            Dropdown categoryButton = GameObject.Find("CategoryDropdown").GetComponent<Dropdown>();
            categoryButton.Select();
            List<string> options = categoryButton.options.ConvertAll(s => s.text);
            bool requirementEngineeringExist = options.Contains("Requirement Engineering");
            bool softwareDesignExist = options.Contains("Software Design");
            bool softwareValidationExist = options.Contains("Software Validation");
            bool softwareMaintenanceExist = options.Contains("Software Maintenance");
            Assert.IsTrue(requirementEngineeringExist && softwareDesignExist &&
            softwareValidationExist && softwareMaintenanceExist);
        }

        [Test]
        public void Campaign_Menu_Load_Video_Test()
        {
            // Use the Assert class to test conditions
            EditorSceneManager.OpenScene("Assets/FPS/Scenes/CampaignMenu.unity");
            Dropdown categoryButton = GameObject.Find("CategoryDropdown").GetComponent<Dropdown>();
            categoryButton.Select();
            CampaignManager campaignManager = GameObject.Find("CampaignManager").GetComponent<CampaignManager>();

            // no category selected
            categoryButton.value = 0;
            campaignManager.setCategory();
            Scene scene = EditorSceneManager.GetActiveScene();
            Assert.AreEqual("CampaignMenu", scene.name);
            Assert.IsTrue(GameObject.Find("CampaignCategoryPopup(Clone)"));

            // valid category selected
            categoryButton.value = 1;
            //campaignManager.setCategory(); // error due to campaignManager line 34 only usable during play mode
            //Button startButton = GameObject.Find("StartButton").GetComponent<Button>();
            //startButton.onClick.Invoke();
            EditorSceneManager.OpenScene("Assets/FPS/Scenes/CampaignVideo.unity");
            Scene next_scene = EditorSceneManager.GetActiveScene();
            Assert.AreEqual("CampaignVideo", next_scene.name);
        }

        [Test]
        public void Custom_Menu_Renders_Test()
        {
            // Use the Assert class to test conditions
            EditorSceneManager.OpenScene("Assets/FPS/Scenes/CustomMenu.unity");
            Assert.IsTrue(GameObject.Find("StartButton") && GameObject.Find("CreateButton") && GameObject.Find("GameIdField"));
        }

        [Test]
        public void Custom_Menu_GameId_Test()
        {
            // Use the Assert class to test conditions
            EditorSceneManager.OpenScene("Assets/FPS/Scenes/CustomMenu.unity");
            GameIdManager.gameId = "60602ffe9e0e3e4dc49cc661";
            EditorSceneManager.OpenScene("Assets/FPS/Scenes/CharScene.unity");
            Assert.IsTrue(GameObject.Find("CharacterList") && GameObject.Find("Confirm") && GameObject.Find("Left") && GameObject.Find("Right"));
        }

        [Test]
        public void Social_Media_Button_Test()
        {
            EditorSceneManager.OpenScene("Assets/FPS/Scenes/SocialMediaScene.unity");
            Assert.IsTrue(GameObject.Find("FacebookShareButton") && GameObject.Find("TwitterShareButton"));
        }

        [Test]
        public void Social_Media_Return_Test()
        {
            EditorSceneManager.OpenScene("Assets/FPS/Scenes/SocialMediaScene.unity");
            LoadSceneButton loadSceneButton = GameObject.Find("CreateButton").GetComponent<LoadSceneButton>();
            Assert.AreEqual("CreateMenu", loadSceneButton.sceneName);

            // Cannot do it the following way because
            // LoadTargetScene calls SceneManager, which shouldn't be used during tests
            // That's why will throw the following error:
            // System.InvalidOperationException : This can only be used during play mode, please use EditorSceneManager.OpenScene() instead.
            //loadSceneButton.LoadTargetScene();
            //Assert.IsTrue(GameObject.Find("CreateMenu"));
        }

        [Test]
        public void View_Leaderboard_Test()
        {
            PlayerLevel.userId = "6038b009801a6c272c853712";
            GameIdManager.gameId = "60602ffe9e0e3e4dc49cc661";
            EditorSceneManager.OpenScene("Assets/FPS/Scenes/Leaderboard_Test.unity");
            Assert.IsTrue(GameObject.Find("Title_Panel"));
        }

        [Test]
        public void Leaderboard_Return_Test()
        {
            EditorSceneManager.OpenScene("Assets/FPS/Scenes/Leaderboard_Test.unity");
            ResetnReturn resetnReturn = GameObject.Find("ResetnReturn").GetComponent<ResetnReturn>();
            resetnReturn.resetVariable();
            Assert.IsTrue((resetnReturn.returnSceneName == "IntroMenu") && (PlayerLevel.level == 1) && (PlayerLevel.score == 0) && (PlayerLevel.wrongQuestionIDs.Count == 0) && (PlayerLevel.upgrade == false) && (PlayerLevel.category == "All") && (PlayerLevel.currentDifficultyIndex == 0));
        }

        [Test]
        public void View_Create_Challenge_Test()
        {
            EditorSceneManager.OpenScene("Assets/FPS/Scenes/CreateMenu.unity");
            Assert.IsTrue(GameObject.Find("CreateButton") && GameObject.Find("QuestionConfigRow") &&
                GameObject.Find("QuestionConfigRow (1)") && GameObject.Find("QuestionConfigRow (2)") &&
                GameObject.Find("QuestionConfigRow (3)") && GameObject.Find("QuestionConfigRow (4)"));
        }

        //[Test]
        //public void Create_Challenge_Test()
        //{
        //    EditorSceneManager.OpenScene("Assets/FPS/Scenes/CreateMenu.unity");
        //    Button createButton = GameObject.Find("CreateButton").GetComponent<Button>();
        //    createButton.onClick.Invoke();
        //    //CreateManager createManager = GameObject.Find("CreateButton").GetComponent<CreateManager>();
        //    //createManager.OnCreate();
        //    Assert.NotNull(CreateManager.createdGameId);
        //}

        [Test]
        public void Create_Challenge_Return_Test()
        {
            EditorSceneManager.OpenScene("Assets/FPS/Scenes/CreateMenu.unity");
            LoadSceneButton loadSceneButton = GameObject.Find("ReturnButton").GetComponent<LoadSceneButton>();
            Assert.AreEqual("CustomMenu", loadSceneButton.sceneName);
        }




    }
}
