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
    private readonly GameChooseMenu _gcm = new(SessionService.SessionsList());
    private readonly object _mainWindow;

    public MainWindow()
    {
        InitializeComponent();

        _mainWindow = Application.Current.MainWindow.Content;

        Application.Current.MainWindow.Content = _gcm;

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
        var genAttr = SessionService.IntRVtoStruct(Character.ListGeneralAttributes);
        var chrData = new Pair<ChrListDataType, MdListDataType>(new ChrListDataType(
                Characters()),
            genAttr
        );
        var mdData = new MdListDataType(
            SessionService.IntRVtoStruct(PSMList.Items.OfType<IReturnValue>().ToList())
        );
        return new SessionDataType(chrData, mdData);
    }

    public ChrListDataType Characters()
    {
        var list = new ChrListDataType();
        var lstchr = lb_players.Items.OfType<CharacterForList>().ToList();
        foreach (var item in lstchr) list.Add(SessionService.ChrSave(item.Character));

        return list;
    }

    public void StartSession()
    {
        var arg = SessionService.LoadSession();
        var chrData = arg.First;
        var mdData = arg.Second;
        Character.ListGeneralAttributes = SessionService.StructRvToInt(chrData.Second);
        if (chrData.First != null)
            foreach (var item in chrData.First)
                lb_players.Items.Add(new CharacterForList(SessionService.ChrLoad(item)));

        if (mdData != null)
            foreach (var item in SessionService.StructRvToInt(mdData))
                PSMList.Items.Add(item);
    }

    public void OpenGameCreationWindow()
    {
        Application.Current.MainWindow.Content = _mainWindow;
    }

    public void OpenGameChoosePage()
    {
        lb_players.Items.Clear();
        PSMList.Items.Clear();
        Application.Current.MainWindow.Content = _gcm;
    }

    internal void AddCharacter(Character character)
    {
        lb_players.Items.Add(new CharacterForList(character));
    }

    public void AddPs(IReturnValue ps)
    {
        ((UIElement)ps).IsEnabled = true;
        PSMList.Items.Add(ps);
    }

    public void RemoveList(bool needToHide)
    {
        var lst = MainGrid.Children.OfType<ListOfUserControls>().ToList();
        foreach (var item in lst) MainGrid.Children.Remove(item);

        if (needToHide) CloseOverlayed();
    }

    public void RemoveCreateCharacter()
    {
        var lst = MainGrid.Children.OfType<CharacterCreate>().ToList();
        foreach (var item in lst) MainGrid.Children.Remove(item);

        CloseOverlayed();

        UnStels();
    }

    public void CreateList(bool isPsList, bool inMainWindow, Character curCh = null)
    {
        var list = new List<IReturnValue>();
        if (isPsList)
            list = SessionService.GetParams();
        else
            list = SessionService.GetAttributes();

        OpenOverlayed(new ListOfUserControls(list, isPsList, inMainWindow, curCh));
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
        OpenOverlayed(new CharacterCreate());
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
        CreateList(false, true);
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

    public void OpenOverlayed(UIElement content)
    {
        Stels();

        GameCreate_grid.Visibility = Visibility.Visible;
        GameCreate_grid.IsEnabled = true;

        Grid.SetRow(content, 1);
        Grid.SetColumn(content, 1);

        GameCreate_grid.Children.Add(content);
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