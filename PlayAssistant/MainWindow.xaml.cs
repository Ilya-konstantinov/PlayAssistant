using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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

        Refrash();
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
            SessionService.IntRVtoStruct(PsmList.Items.OfType<IReturnValue>().ToList())
        );
        return new SessionDataType(chrData, mdData);
    }

    public ChrListDataType Characters()
    {
        var list = new ChrListDataType();
        var lstchr = LbPlayers.Items.OfType<CharacterForList>().ToList();
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
                LbPlayers.Items.Add(new CharacterForList(SessionService.ChrLoad(item)));

        if (mdData != null)
            foreach (var item in SessionService.StructRvToInt(mdData))
                PsmList.Items.Add(item);
    }

    public void OpenGameCreationWindow()
    {
        Application.Current.MainWindow.Content = _mainWindow;
    }

    public void OpenGameChoosePage()
    {
        LbPlayers.Items.Clear();
        PsmList.Items.Clear();
        Application.Current.MainWindow.Content = new GameChooseMenu(SessionService.SessionsList());
    }

    internal void AddCharacter(Character character)
    {
        LbPlayers.Items.Add(new CharacterForList(character));
    }

    public void AddPs(IReturnValue ps)
    {
        ((UIElement)ps).IsEnabled = true;
        PsmList.Items.Add(ps);
    }

    public void RemoveList(bool needToHide)
    {
        var lst = PsmPicker.Children.OfType<ListOfUserControls>().ToList();
        foreach (var item in lst) PsmPicker.Children.Remove(item);
        var another_lst = GameCreateGrid.Children.OfType<ListOfUserControls>().ToList();
        foreach (var item in another_lst) GameCreateGrid.Children.Remove(item);

        if (needToHide) CloseOverlayed();
    }

    public void RemoveCreateCharacter()
    {
        var lst = PsmPicker.Children.OfType<CharacterCreate>().ToList();
        foreach (var item in lst) PsmPicker.Children.Remove(item);

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
        CreateList(false, true);
    }

    public void Refrash()
    {
        foreach (var item in GameCreateGrid.Children.OfType<CharacterCreate>()) item.Refrash();

        foreach (var item in LbPlayers.Items.OfType<CharacterForList>()) item.Refresh();
    }

    private void Button_Click_3(object sender, RoutedEventArgs e)
    {
        CreateList(true, true);
    }

    public void OpenGameCreate(object sender, RoutedEventArgs e)
    {
        OpenOverlayed(new GameCreateMenu());
    }

    public void CloseGameCreate()
    {
        GameCreateGrid.Visibility = Visibility.Hidden;
        GameCreateGrid.IsEnabled = false;

        GameCreateGrid.Children.Clear();

        UnStels();
    }

    public void OpenOverlayed(UIElement content)
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
        Refrash();
        SessionService.SaveSession(SessionData());
        SessionService.SessionName = null;
        OpenGameChoosePage();
    }
}