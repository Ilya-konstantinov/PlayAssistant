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
using MdListDataType = List<Pair<Type, ReturnValue>>;
using ChrListDataType = List<CharacterBase>;
using SessionDataType =
    Pair<Pair<List<CharacterBase>, List<Pair<Type, ReturnValue>>>, List<Pair<Type, ReturnValue>>>;

public partial class MainWindow : Window
{
    private readonly GameChooseMenu gcm = new(SessionService.SessionsList());
    private readonly object mainWindow;

    public MainWindow()
    {
        InitializeComponent();

        mainWindow = Application.Current.MainWindow.Content;

        Application.Current.MainWindow.Content = gcm;

        Closing += MainWindow_Closing;
    }

    //  1 -- персонажи 2 -- лист генеральных характеристик 3 -- лист игровых модулей
    // Pair<Pair<ChrListDataType, MdListDataType> MdListDataType>
    private void MainWindow_Closing(object? sender, CancelEventArgs e)
    {
        if (SessionService.SessionName == null) return;

        
        SessionService.SaveSession(SessionData());
    }

    public SessionDataType SessionData()
    {
        var GenAttr = SessionService.IntRVtoStruct(Character.ListGeneralAttributes);
        var ChrData = new Pair<ChrListDataType, MdListDataType>(new ChrListDataType(
                characters()),
            GenAttr
        );
        var MdData = new MdListDataType(
            SessionService.IntRVtoStruct(PSMList.Items.OfType<IReturnValue>().ToList())
        );
        return new SessionDataType(ChrData, MdData);
    }

    public ChrListDataType characters()
    {
        var list = new ChrListDataType();
        var lstchr = lb_players.Items.OfType<CharacterForList>().ToList();
        foreach (var item in lstchr) list.Add(SessionService.ChrSave(item.character));

        return list;
    }

    public void StartSession()
    {
        var arg = SessionService.LoadSession();
        var ChrData = arg.First;
        var MdData = arg.Second;
        Character.ListGeneralAttributes = SessionService.StructRVToInt(ChrData.Second);
        if (ChrData.First != null)
            foreach (var item in ChrData.First)
                lb_players.Items.Add(new CharacterForList(SessionService.ChrLoad(item)));

        if (MdData != null)
            foreach (var item in SessionService.StructRVToInt(MdData))
                PSMList.Items.Add(item);
    }

    public void OpenGameCreationWindow()
    {
        Application.Current.MainWindow.Content = mainWindow;
    }

    public void OpenGameChoosePage()
    {
        lb_players.Items.Clear();
        PSMList.Items.Clear();
        Application.Current.MainWindow.Content = gcm;
    }

    internal void AddCharacter(Character character)
    {
        lb_players.Items.Add(new CharacterForList(character));
    }

    public void AddPS(IReturnValue PS)
    {
        PSMList.Items.Add(PS);
    }

    public void RemoveList(bool NeedToHide)
    {
        var lst = MainGrid.Children.OfType<ListOfUserControls>().ToList();
        foreach (var item in lst) PSMPicker.Children.Remove(item);

        if (NeedToHide) UnStels();
    }

    public void RemoveCreateCharacter()
    {
        var lst = MainGrid.Children.OfType<CharacterCreate>().ToList();
        foreach (var item in lst) PSMPicker.Children.Remove(item);

        UnStels();
    }

    public void CreateList(bool IsPSList, bool InMainWindow, Character curCh = null)
    {
        var list = new List<IReturnValue>();
        if (IsPSList)
            list = SessionService.GetParams();
        else
            list = SessionService.GetAttributes();

        var listOfUserControls = new ListOfUserControls(list, IsPSList, InMainWindow, curCh);
        OpenOverlayed(listOfUserControls);
    }

    public void Stels()
    {
        Hide.IsEnabled = true;
        Hide.Visibility = Visibility.Visible;
    }

    public void UnStels()
    {
        Hide.IsEnabled = false;
        Hide.Visibility = Visibility.Hidden;
    }

    private void btn1_Click(object sender, RoutedEventArgs e)
    {
        Stels();
        var characterCreate = new CharacterCreate();
        characterCreate.SetValue(Grid.ColumnProperty, 2);
        characterCreate.SetValue(Grid.RowProperty, 1);
        MainGrid.Children.Add(characterCreate);
    }

    private void Button_Click_2(object sender, RoutedEventArgs e)
    {
        CreateList(true, true);
    }

    public void Refrash()
    {
        foreach (var item in MainGrid.Children.OfType<CharacterCreate>()) item.Refrash();

        foreach (var item in lb_players.Items.OfType<CharacterForList>()) item.Refresh();
    }

    private void Button_Click_3(object sender, RoutedEventArgs e)
    {
        Stels();
        CreateList(true, true);
    }

    public void OpenGameCreate(object sender, RoutedEventArgs e)
    {
        OpenOverlayed(new GameCreateMenu());
    }

    public void CloseGameCreate()
    {
        GameCreate_grid.Visibility = Visibility.Hidden;
        GameCreate_grid.IsEnabled = false;

        GameCreate_grid.Children.Clear();

        UnStels();
    }

    public void OpenOverlayed(UIElement _content)
    {
        Stels();

        GameCreate_grid.Visibility = Visibility.Visible;
        GameCreate_grid.IsEnabled = true;

        Grid.SetRow(_content, 1);
        Grid.SetColumn(_content, 1);

        GameCreate_grid.Children.Add(_content);
    }

    public void CloseOverlayed()
    {
        GameCreate_grid.Visibility = Visibility.Hidden;
        GameCreate_grid.IsEnabled = false;

        GameCreate_grid.Children.Clear();

        UnStels();
    }

    private void Exit_btn_Click(object sender, RoutedEventArgs e)
    {
        SessionService.SaveSession(SessionData());
        SessionService.SessionName = null;
        OpenGameChoosePage();
    }
}