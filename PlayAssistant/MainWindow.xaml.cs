using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ServiceLibrary;

namespace PlayAssistant;

/// <summary>
/// Логика взаимодействия для MainWindow.xaml
/// </summary>
using ModuleListDataType = List<Pair<Type, ReturnValue>>;
using CharacterListDataType = List<CharacterBase>;
using SessionDataType =
    Pair<Pair<List<CharacterBase>, List<Pair<Type, ReturnValue>>>, List<Pair<Type, ReturnValue>>>;

public partial class MainWindow
{
    private readonly object _mainWindow;

    public MainWindow()
    {
        InitializeComponent();

        _mainWindow = Application.Current.MainWindow.Content;

        Application.Current.MainWindow.Content = new GameChooseMenu(SessionService.SessionsList());

        Closing += MainWindow_Closing;
    }

    //  1 -- персонажи 2 -- лист генеральных характеристик 3 -- лист игровых модулей
    // Pair<Pair<ChrListDataType, MdListDataType> MdListDataType>
    private void MainWindow_Closing(object? sender, CancelEventArgs e)
    {
        if (SessionService.SessionName == null) return;

        Refresh();
        SessionService.SaveSession(SessionData());
    }

    private SessionDataType SessionData()
    {
        var generalAttributes = SessionService.InterfaceToStructure(Character.ListGeneralAttributes);
        var chrData = new Pair<CharacterListDataType, ModuleListDataType>(new CharacterListDataType(
                Characters()),
            generalAttributes
        );
        var moduleData = new ModuleListDataType(
            SessionService.InterfaceToStructure(ListOfPlayStaticModules.Items.OfType<IReturnValue>().ToList())
        );
        return new SessionDataType(chrData, moduleData);
    }

    private CharacterListDataType Characters()
    {
        var list = new CharacterListDataType();
        var listOfCharacters = ListOfPlayers.Items.OfType<CharacterForList>().ToList();
        foreach (var item in listOfCharacters) list.Add(SessionService.ChrSave(item.Character));

        return list;
    }

    public void StartSession()
    {
        var arguments = SessionService.LoadSession();
        var characterData = arguments.First;
        var moduleData = arguments.Second;
        Character.ListGeneralAttributes = SessionService.StructureToInterface(characterData.Second);
        if (characterData.First != null)
            foreach (var item in characterData.First)
                ListOfPlayers.Items.Add(new CharacterForList(SessionService.ChrLoad(item)));

        if (moduleData != null)
            foreach (var item in SessionService.StructureToInterface(moduleData))
                ListOfPlayStaticModules.Items.Add(item);
    }

    public void OpenGameCreationWindow()
    {
        Application.Current.MainWindow.Content = _mainWindow;
    }

    private void OpenGameChoosePage()
    {
        ListOfPlayers.Items.Clear();
        ListOfPlayStaticModules.Items.Clear();
        Application.Current.MainWindow.Content = new GameChooseMenu(SessionService.SessionsList());
    }

    internal void AddCharacter(Character character)
    {
        ListOfPlayers.Items.Add(new CharacterForList(character));
    }

    public void AddPlayStatic(IReturnValue playStatic)
    {
        ((UIElement)playStatic).IsEnabled = true;
        ListOfPlayStaticModules.Items.Add(playStatic);
    }

    public void RemoveList(bool needToHide)
    {
        var listOfUserControls = PlayStaticModulesPicker.Children.OfType<ListOfUserControls>().ToList();
        foreach (var item in listOfUserControls)
        {
            PlayStaticModulesPicker.Children.Remove(item);
        }
        
        var another_lst = GameCreateGrid.Children.OfType<ListOfUserControls>().ToList();
        foreach (var item in another_lst)
        {
            GameCreateGrid.Children.Remove(item);
        }

        if (needToHide) CloseOverlayed();
    }

    public void RemoveCharacterCreationPanel()
    {
        var listOfCharacterCreates = PlayStaticModulesPicker.Children.OfType<CharacterCreate>().ToList();
        foreach (var item in listOfCharacterCreates) PlayStaticModulesPicker.Children.Remove(item);

        CloseOverlayed();

        UnStels();
    }

    public void CreateList(bool isPlayStaticList, bool inMainWindow, Character currentCharacter = null)
    {
        var list = new List<IReturnValue>();
        if (isPlayStaticList)
            list = SessionService.GetParams();
        else
            list = SessionService.GetAttributes();

        OpenOverlayed(new ListOfUserControls(list, isPlayStaticList, inMainWindow, currentCharacter));
    }

    private void Stels()
    {
        Hide.IsEnabled = true;
        Hide.Visibility = Visibility.Visible;
    }

    private void UnStels()
    {
        Hide.IsEnabled = false;
        Hide.Visibility = Visibility.Hidden;
    }

    private void CreateCharacter_Click(object sender, RoutedEventArgs e)
    {
        OpenOverlayed(new CharacterCreate());
    }

    private void AddGlobalCharacteristic_Click(object sender, RoutedEventArgs e)
    {
        CreateList(false, true);
    }

    public void Refresh()
    {
        foreach (var item in GameCreateGrid.Children.OfType<CharacterCreate>()) item.Refresh();

        foreach (var item in ListOfPlayers.Items.OfType<CharacterForList>()) item.Refresh();
    }

    private void AddGameStatistic(object sender, RoutedEventArgs e)
    {
        CreateList(true, true);
    }

    private void OpenOverlayed(UIElement content)
    {
        Stels();

        GameCreateGrid.Visibility = Visibility.Visible;
        GameCreateGrid.IsEnabled = true;

        Grid.SetRow(content, 1);
        Grid.SetColumn(content, 1);

        GameCreateGrid.Children.Add(content);
    }

    public void CloseOverlayed()
    {
        GameCreateGrid.Visibility = Visibility.Hidden;
        GameCreateGrid.IsEnabled = false;

        GameCreateGrid.Children.Clear();

        UnStels();
    }

    private void Exit_btn_Click(object sender, RoutedEventArgs e)
    {
        Refresh();
        SessionService.SaveSession(SessionData());
        SessionService.SessionName = null;
        OpenGameChoosePage();
    }
}