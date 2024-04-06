﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Itel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const string path = @"D:\Dropbox\Text Files\start cards.txt";
        const string mahPath = @"D:\Dropbox\Text Files\Serials.txt";
        public const string verPath = "version.txt";

        ItelUser user1 = new ItelUser("sdk-a", false);
        //ItelUser user2 = new ItelUser("sdk-m");
        ItelUser user2 = new ItelUser("sdk-a", true);
        ItelUser mainUser;

        public static string ItelVer = "3230";

        double balance = 0;
        double purchasingBalance = 0;
        public static double ExtraPrice = 0;
        int sayrafaRate = 0;
        double rebateRateTouch = 0;
        double rebateRateAlfa = 0;
        double rebateRateTouchClass = 0;
        double rebateRateAlfaClass = 0;

        double sosPrice = 1.37; // 4.21;
        double startPrice = 5.01; //10.26;
        double alfaSmallPrice = 3.38;
        double touchSmallPrice = 4.22; //12.78;
        double touchMediumPrice = 5.01; //19.11;
        double oneMonthPrice = 8.43; //25.4;
        double twoMonthPrice = 16.83; //50.62;
        double threeMonthPrice = 25.25; //75.85;
        double touchYearPrice = 85.8; //257.49;


        //bool touchSmallEnabled = false;
        string tempPath;

        List<string> lines;
        bool changed = false;
        bool windowCanBeClosed = true;

        public MainWindow()
        {
            InitializeComponent();
            if (File.Exists(verPath))
            {
                string[] data = File.ReadAllLines(verPath);
                ItelVer = data[0];
                ExtraPrice = double.Parse(data[1]);
                sayrafaRate = int.Parse(data[2]);
                rebateRateTouch = double.Parse(data[3]);
                rebateRateAlfa = double.Parse(data[4]);

                rebateRateTouchClass = double.Parse(data[6]);
                rebateRateAlfaClass = double.Parse(data[7]);
            }
            else
            {
                string[] lines = new string[] { ItelVer, ExtraPrice.ToString() };
                File.WriteAllLines(verPath, lines);
            }
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            goButton.Tag = false;
            classButton.Tag = true;
            /* old for whish
            check.IsEnabled = true;

            //mainGrid.UpdateLayout();
            //RenderTargetBitmap image = new RenderTargetBitmap((int)mainGrid.ActualWidth + 4, (int)mainGrid.ActualHeight + 4, 120, 96, PixelFormats.Pbgra32);
            //image.Render(mainGrid);
            //Clipboard.SetImage(image);

            try
            {
                mainUser = user1;

                await mainUser.GetSessioncounter();

                if (mainUser.SessionCounter == "update")
                {
                    statusTB.Foreground = Brushes.Red;
                    statusTB.Text = "There is an update\nPlease Restart";
                    int ver = int.Parse(ItelVer);
                    ver += 10;
                    File.WriteAllLines(verPath, new string[] { ver.ToString(), ExtraPrice.ToString() });
                    return;
                }
                else if (mainUser.SessionCounter == null)
                {
                    statusTB.Foreground = Brushes.Red;
                    statusTB.Text = "**Error**\nApp in maintenance";
                    return;
                }
                else if (mainUser.SessionCounter == "error")
                {
                    statusTB.Foreground = Brushes.Red;
                    statusTB.Text = "Can't open program";
                    return;

                }
                else if (mainUser.SessionCounter == "restart")
                {
                    statusTB.Foreground = Brushes.Red;
                    statusTB.Text = "Please restart the program";
                    return;

                }

                await GetBalance();
                BTNdetail.IsEnabled = true;
                buttonOldCards.IsEnabled = true;
            }
            catch (WebException ex)
            {
                statusTB.Foreground = Brushes.Red;
                statusTB.Text = "No Internet";
            }
            catch (Exception ex)
            {
                statusTB.Foreground = Brushes.Red;
                statusTB.Text = ex.Message;
            }*/
        }

        async public Task GetBalance(double b = 0)
        {
            if (b == 0)
            {
                balance = await mainUser.GetBalance();
            }
            else
                balance -= b;

            if (balance < 1000)
                balanceTB.Foreground = Brushes.Red;

            if (balance > 150)
            {
                startProgTB.IsEnabled = true;
                sosProgTB.IsEnabled = true;
                touchSmallProgTB.IsEnabled = true;
                touch1MonthProgTB.IsEnabled = true;
                touchMediumProgTB.IsEnabled = true;
                //check.IsEnabled = true;
                //touch1MonthProgTB.Focus();
            }

            if (balance > 300)
            {
                touch2MonthProgTB.IsEnabled = true;
            }

            if (balance > 450)
            {
                touch3MonthProgTB.IsEnabled = true;
            }

            if (balance > 1500)
            {
                touchYearProgTB.IsEnabled = true;
            }

            balanceTB.Text = string.Format("Balance = {0:0.000} $", balance);
            balanceTB.Visibility = Visibility.Visible;
        }

        async private void goBTN_Click(object sender, RoutedEventArgs e)
        {
            goButton.IsEnabled = false;
            classButton.IsEnabled = false;
            windowCanBeClosed = false;
            //bool isForMah = check.IsChecked == true;
            //tempPath = isForMah ? mahPath : path;

            try
            {
                if (changed)
                {
                    purchasingBalance = int.Parse(touch1MonthProgTB.Text) * (oneMonthPrice + ExtraPrice) +
                        int.Parse(touch2MonthProgTB.Text) * (twoMonthPrice + ExtraPrice) +
                        int.Parse(touch3MonthProgTB.Text) * (threeMonthPrice + ExtraPrice) +
                        int.Parse(touchSmallProgTB.Text) * (touchSmallPrice + ExtraPrice) +
                        int.Parse(touchMediumProgTB.Text) * (touchMediumPrice + ExtraPrice) +
                        int.Parse(startProgTB.Text) * (startPrice + ExtraPrice) +
                        int.Parse(sosProgTB.Text) * (sosPrice + ExtraPrice) +
                        int.Parse(alfa1MonthProgTB.Text) * (oneMonthPrice + ExtraPrice) +
                        int.Parse(alfaSmallProgTB.Text) * (alfaSmallPrice + ExtraPrice) +
                        int.Parse(touchYearProgTB.Text) * (touchYearPrice + ExtraPrice);

                    if (balance > purchasingBalance)
                    {
                        lines = new List<string>(File.ReadAllLines(tempPath));

                        statusTB.Text = "Purchasing touch 7.5";
                        await purchase(touch1MonthProgTB);
                        statusTB.Text = "Purchasing touch 15";
                        await purchase(touch2MonthProgTB);
                        statusTB.Text = "Purchasing touch 22.7";
                        await purchase(touch3MonthProgTB);
                        statusTB.Text = "Purchasing touch 3.79";
                        await purchase(touchSmallProgTB);
                        statusTB.Text = "Purchasing touch 4.5";
                        await purchase(touchMediumProgTB);
                        statusTB.Text = "Purchasing alfa 7.5";
                        await purchase(alfa1MonthProgTB);
                        //statusTB.Text = "Purchasing alfa 50";
                        //await purchase(alfa2MonthProgTB);
                        //statusTB.Text = "Purchasing alfa 75";
                        //await purchase(alfa3MonthProgTB);
                        statusTB.Text = "Purchasing alfa 3";
                        await purchase(alfaSmallProgTB);
                        statusTB.Text = "Purchasing start";
                        await purchase(startProgTB);
                        statusTB.Text = "Purchasing SOS";
                        await purchase(sosProgTB);
                        statusTB.Text = "Purchasing touch YEAR";
                        await purchase(touchYearProgTB);


                        purchasingBalance = int.Parse(touch1MonthProgTB.Text) * (oneMonthPrice + ExtraPrice) +
                        int.Parse(touch2MonthProgTB.Text) * (twoMonthPrice + ExtraPrice) +
                        int.Parse(touch3MonthProgTB.Text) * (threeMonthPrice + ExtraPrice) +
                        int.Parse(touchSmallProgTB.Text) * (touchSmallPrice + ExtraPrice) +
                        int.Parse(touchMediumProgTB.Text) * (touchMediumPrice + ExtraPrice) +
                        int.Parse(startProgTB.Text) * (startPrice + ExtraPrice) +
                        int.Parse(sosProgTB.Text) * (sosPrice + ExtraPrice) +
                        int.Parse(alfa1MonthProgTB.Text) * (oneMonthPrice + ExtraPrice) +
                        int.Parse(alfaSmallProgTB.Text) * (alfaSmallPrice + ExtraPrice) +
                        int.Parse(touchYearProgTB.Text) * (touchYearPrice + ExtraPrice);

                        await GetBalance(purchasingBalance);
                    }
                    else
                    {
                        statusTB.Foreground = Brushes.Red;
                        statusTB.Text = "No sufficient balance !!";
                        return;
                    }
                }

                //if (!isForMah)
                updateVouchersTextFile((bool)(sender as Button).Tag);

                windowCanBeClosed = true;
                statusTB.Text = "Done";
                statusTB.Foreground = Brushes.Green;

                //balance -= purchasingBalance;
                //this.Title = string.Format("Balance = {0:0.000}", balance);
            }
            catch (WebException ex)
            {
                statusTB.Foreground = Brushes.Red;
                statusTB.Text = "No Internet";
                windowCanBeClosed = true;
            }
            catch (FormatException ex)
            {
                statusTB.Foreground = Brushes.Red;
                statusTB.Text = "Error in card count";
                //if (!isForMah)
                //    updateVouchersTextFile();
                windowCanBeClosed = true;
            }
            catch (Exception ex)
            {

                statusTB.Foreground = Brushes.Red;
                statusTB.Text = ex.Message;
                windowCanBeClosed = true;
            }
        }

        async private Task purchase(TextBox tb)
        {
            try
            {
                string domId = tb.Tag as string;
                string countString = tb.Text;
                string cardType;
                string price = "0";
                bool bigCard = false;


                int count = int.Parse(countString);
                if (count == 0) return;

                switch (domId)
                {
                    case "80011":
                        cardType = "to(12.5)uch";
                        price = "1936200";
                        break;

                    case "80017":
                        cardType = "to(19.11)uch";
                        price = "2895200";
                        break;

                    case "80022":
                        cardType = "touch25";
                        price = "3848100";
                        break;

                    case "80045":
                        cardType = "touch50";
                        price = "7669000";
                        bigCard = true;
                        break;

                    case "80068":
                        cardType = "touch75";
                        price = "11491300";
                        bigCard = true;
                        break;

                    case "80231":
                        cardType = "touchYear";
                        price = "39009800";
                        bigCard = true;
                        break;

                    case "80010":
                        cardType = "start";
                        price = "1554400";
                        break;

                    case "80040":
                        cardType = "SOS";
                        price = "637900";
                        break;

                    case "81025":
                        cardType = "alfa25";
                        price = "3848100";
                        break;

                    //case "81050":
                    //    cardType = "alfa50";
                    //    bigCard = true;
                    //    break;

                    //case "81075":
                    //    cardType = "alfa75";
                    //    bigCard = true;
                    //    break;

                    case "81010":
                        cardType = "al(9.09)fa";
                        price = "1554400";
                        break;

                    default:
                        cardType = "";
                        break;
                }

                if (count > 16 || (bigCard && count > 2))
                {
                    setNoCards(tb, "high cards count not allowed");
                    return;
                }


                if (bigCard && MessageBox.Show(string.Format("Are you sure to issue {0} cards of {1} ?", count, cardType), "Big Card",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No, MessageBoxOptions.ServiceNotification) == MessageBoxResult.No)
                {
                    setNoCards(tb, "Cancelled");
                    return;
                }

                await Task.Delay(1000);

                List<Voucher> vouchers = await mainUser.PurchaseVouchers(domId, countString, price);


                int lineNumber = 0;

                if (check.IsChecked == true)
                {
                    int l = (cardType.StartsWith("al") ? 1 : 0);
                    int mahCount = int.Parse(lines[l]);
                    mahCount += count;
                    lines[l] = mahCount.ToString();
                }

                foreach (string l in lines)
                {
                    if (l.Contains(cardType))
                    {
                        lineNumber += 2;
                        break;
                    }
                    else
                        lineNumber++;
                }

                int tempCount = vouchers.Count;
                //List<Voucher> newVouchersList = new List<Voucher>(vouchers);
                //newVouchersList[tempCount - 1] = vouchers[0];

                //int d = 1;
                //for (int c = 1; c < tempCount; c++)
                //{
                //    int pos = tempCount - 1 - c - d;
                //    if (pos < 0) pos = 0;
                //    else { if (c == 1 || c == 2) pos += d; }
                //    newVouchersList[pos] = vouchers[c];
                //    d *= -1;
                //}

                foreach (Voucher v in vouchers)
                {
                    string formatted = string.Format("{0}. {1}", tempCount, ReformatVoucher(v.secret, (cardType.StartsWith("al"))));
                    string placeholder = "";
                    if (domId == "80022" || domId == "80010" || domId == "80011")
                        placeholder = " ,    " + v.serial;
                    else if (domId == "81010")
                        placeholder = " , (alfa z8ir)";
                    else if (domId == "80017")
                        placeholder = " , (كرت وسط)";
                    lines.Insert(lineNumber, formatted + placeholder + "     [" + v.secret + "]");
                    tempCount--;
                }


                lineNumber += count;

                while (lines[lineNumber].Contains("."))
                {
                    count++;
                    string line = lines[lineNumber];
                    line = Regex.Replace(line, "^\\d{1,2}.", count.ToString() + ".");
                    lines[lineNumber++] = line;
                }

                tb.Foreground = Brushes.Green;

                File.WriteAllLines(tempPath, lines);
            }
            catch
            {
                setNoCards(tb, "No cards");
            }

        }

        private void updateVouchersTextFile(bool isClass)
        {
            double touchRebate = isClass ? rebateRateTouchClass : rebateRateTouch;
            double alfaRebate = isClass ? rebateRateAlfaClass : rebateRateAlfa;
            int sayrafa = isClass ? sayrafaRate - 100 : sayrafaRate;

            string path = @"D:\Dropbox\Grandstream new\Progs\JaroorDolarat.txt";
            int touchSmallCount = int.Parse(touchSmallCardTB.Text) + int.Parse(touchSmallProgTB.Text);
            int touchMediumCount = int.Parse(touchMediumCardTB.Text) + int.Parse(touchMediumProgTB.Text);
            int touch1MonthCount = int.Parse(touchBigCardTB.Text) + int.Parse(touch1MonthProgTB.Text);
            int touch2MonthCount = int.Parse(touch2MonthCardTB.Text) + int.Parse(touch2MonthProgTB.Text);
            int touch3MonthCount = int.Parse(touch3MonthCardTB.Text) + int.Parse(touch3MonthProgTB.Text);
            int touchYearCount = int.Parse(touchYearCardTB.Text) + int.Parse(touchYearProgTB.Text);
            int alfaSmallCount = int.Parse(alfaSmallCardTB.Text) + int.Parse(alfaSmallProgTB.Text);
            int alfaMediumCount = int.Parse(alfaMediumCardTB.Text);
            int alfa1MonthCount = int.Parse(alfaBigCardTB.Text) + int.Parse(alfa1MonthProgTB.Text);
            //int alfa2MonthCount = int.Parse(alfa2MonthProgTB.Text);
            //int alfa3MonthCount = int.Parse(alfa3MonthProgTB.Text);
            int sosCount = int.Parse(sosCardTB.Text) + int.Parse(sosProgTB.Text);
            int startCount = int.Parse(startCardTB.Text) + int.Parse(startProgTB.Text);
            int mediumCount = touchMediumCount + alfaMediumCount;

            double totalTouchSum = touchSmallCount * (touchSmallPrice + ExtraPrice) + touchMediumCount * (touchMediumPrice + ExtraPrice)
                + touch1MonthCount * (oneMonthPrice + ExtraPrice) + touchYearCount * (touchYearPrice + ExtraPrice)
                + touch2MonthCount * (twoMonthPrice + ExtraPrice) + touch3MonthCount * (threeMonthPrice + ExtraPrice);
            totalTouchSum = totalTouchSum * sayrafa * touchRebate / 1500;
            totalTouchSum = Math.Round(totalTouchSum, 2);
            double totalAlfaSum = alfaSmallCount * (alfaSmallPrice + ExtraPrice) + alfaMediumCount * (touchMediumPrice + ExtraPrice)
                + alfa1MonthCount * (oneMonthPrice + ExtraPrice);
            totalAlfaSum = totalAlfaSum * sayrafa * alfaRebate / 1500;
            totalAlfaSum = Math.Round(totalAlfaSum, 2);
            double totalStartSum = sosCount * (sosPrice + ExtraPrice) + startCount * (startPrice + ExtraPrice);
            totalStartSum = totalStartSum * sayrafa * touchRebate / 1500;
            totalStartSum = Math.Round(totalStartSum, 2);
            double totalSum = totalTouchSum + totalAlfaSum + totalStartSum;


            int cashLine = 0;
            int cardsLine = 0;
            int purchasesLine = 0;

            List<string> lines = new List<string>(File.ReadAllLines(path));
            lines.Reverse();

            int lineNumber = 0;
            foreach (string l in lines)
            {
                if (l.StartsWith("cards"))
                {
                    cardsLine = lineNumber;
                    cashLine = lineNumber + 1;
                    purchasesLine = lineNumber + 5;
                    break;
                }
                else
                    lineNumber++;
            }
            // string dayFormat = @"cards\t: (.*?) \((\d{1,2})\+(\d{1,2})\+(\d{1,2})\+(\d{1,2})\.MTC  (\d{1,2})\+(\d{1,2})\+(\d{1,2})\+(\d{1,2})\.Alfa\) \+ (.*?)\((\d{1,2})\.St  (\d{1,2})\.SOS\)";

            string cardsFormat = "cards	: {0:0.00} ({1}s+{2}m+{3}b,{4}+{5}+{6}y.MTC  {7}+{8}.Alfa) + {9:0.00}({10}.St  {11}.SOS)";
            Match match = Regex.Match(lines[cardsLine], @"cards\t: (.*?) \((\d{1,2})s\+(\d{1,2})m\+(\d{1,2})b\,(\d{1,2})\+(\d{1,2})\+(\d{1,2})y\.MTC  (\d{1,2})\+(\d{1,2})\.Alfa\) \+ (.*?)\((\d{1,2})\.St  (\d{1,2})\.SOS\)");
            double text_total1 = double.Parse(match.Groups[1].Value);
            text_total1 += totalTouchSum + totalAlfaSum;
            double text_total2 = double.Parse(match.Groups[10].Value);
            text_total2 += totalStartSum;
            int text_touchSmallCount = int.Parse(match.Groups[2].Value);
            text_touchSmallCount += touchSmallCount;
            int text_touchMediumCount = int.Parse(match.Groups[3].Value);
            text_touchMediumCount += mediumCount;
            int text_touch1MonthCount = int.Parse(match.Groups[4].Value);
            text_touch1MonthCount += touch1MonthCount;
            int text_touch2MonthCount = int.Parse(match.Groups[5].Value);
            text_touch2MonthCount += touch2MonthCount;
            int text_touch3MonthCount = int.Parse(match.Groups[6].Value);
            text_touch3MonthCount += touch3MonthCount;
            int text_touchYearCount = int.Parse(match.Groups[7].Value);
            text_touchYearCount += touchYearCount;
            int text_alfaSmallCount = int.Parse(match.Groups[8].Value);
            text_alfaSmallCount += alfaSmallCount;
            int text_alfa1MonthCount = int.Parse(match.Groups[9].Value);
            text_alfa1MonthCount += alfa1MonthCount;
            //int text_alfa2MonthCount = int.Parse(match.Groups[8].Value);
            //text_alfa2MonthCount += alfa2MonthCount;
            //int text_alfa3MonthCount = int.Parse(match.Groups[9].Value);
            //text_alfa3MonthCount += alfa3MonthCount;
            int text_startCount = int.Parse(match.Groups[11].Value);
            text_startCount += startCount;
            int text_sosCount = int.Parse(match.Groups[12].Value);
            text_sosCount += sosCount;
            lines[cardsLine] = string.Format(cardsFormat, text_total1, text_touchSmallCount, text_touchMediumCount, text_touch1MonthCount,
                text_touch2MonthCount, text_touch3MonthCount, text_touchYearCount, text_alfaSmallCount, text_alfa1MonthCount,
                text_total2, text_startCount, text_sosCount);

            //string cashFormat = "cash	: 968.66+50 (5asara 0$) + 86.33";
            if (isClass)
            {
                var cs = Regex.Match(lines[cashLine], @"\+ (\d{1,5}(?:\.\d{1,2})?)").Groups;
                var mat = cs[1];
                double change = double.Parse(mat.Value, System.Globalization.NumberStyles.Float);
                change -= totalSum;
                string replacement = "+ " + change.ToString("0.00");
                lines[cashLine] = Regex.Replace(lines[cashLine], @"\+ \d{1,5}(?:\.\d{1,2})?", replacement);

                lines.Reverse();
                File.WriteAllLines(path, lines);
                return;
            }
            else
            {
                var mat = Regex.Match(lines[cashLine], @"[+-]\d{1,5}(?:\.\d{1,2})?");
                double change = double.Parse(mat.Value, System.Globalization.NumberStyles.Float);
                change -= totalSum;
                string replacement = (change < 0 ? "" : "+") + change.ToString("0.00");
                lines[cashLine] = Regex.Replace(lines[cashLine], @"[+-]\d{1,5}(?:\.\d{1,2})?", replacement);
            }

            if (!lines[purchasesLine].StartsWith("("))
            {
                lines.Insert(purchasesLine, string.Format("({0}s+{1}m+{2}b,{3}+{4}+{5}y {6}+{7} {8}+{9} = {10})",
                     touchSmallCount, mediumCount, touch1MonthCount, touch2MonthCount, touch3MonthCount, touchYearCount,
                     alfaSmallCount, alfa1MonthCount, startCount, sosCount, totalSum));
            }
            else
            {
                match = Regex.Match(lines[purchasesLine], @"\((\d{1,2})s\+(\d{1,2})m\+(\d{1,2})b\,(\d{1,2})\+(\d{1,2})\+(\d{1,2})y (\d{1,2})\+(\d{1,2}) (\d{1,2})\+(\d{1,2}) = (\d{1,5}(?:\.\d{1,2})?)\)");
                int t1 = int.Parse(match.Groups[1].Value) + touchSmallCount;
                int t2 = int.Parse(match.Groups[2].Value) + mediumCount;
                int t3 = int.Parse(match.Groups[3].Value) + touch1MonthCount;
                int t4 = int.Parse(match.Groups[4].Value) + touch2MonthCount;
                int t5 = int.Parse(match.Groups[5].Value) + touch3MonthCount;
                int t6 = int.Parse(match.Groups[6].Value) + touchYearCount;
                int a1 = int.Parse(match.Groups[7].Value) + alfaSmallCount;
                int a2 = int.Parse(match.Groups[8].Value) + alfa1MonthCount;
                int s1 = int.Parse(match.Groups[9].Value) + startCount;
                int s2 = int.Parse(match.Groups[10].Value) + sosCount;
                double total = double.Parse(match.Groups[11].Value) + totalSum;

                lines[purchasesLine] = string.Format("({0}s+{1}m+{2}b,{3}+{4}+{5}y {6}+{7} {8}+{9} = {10})",
                    t1, t2, t3, t4, t5, t6, a1, a2, s1, s2, total);
            }

            lines.Reverse();

            File.WriteAllLines(path, lines);
        }

        private string ReformatVoucher(string voucher, bool isAlfa)
        {
            StringBuilder reformatted = new StringBuilder();
            int spaceLocation = isAlfa ? 3 : 2;

            int i = 0;
            foreach (char c in voucher)
            {
                reformatted.Append(c);
                if (++i % spaceLocation == 0)
                    reformatted.Append(" ");
            }

            return reformatted.ToString().TrimEnd(' ');
        }

        private void textbox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            tb.SelectAll();
        }

        private void StackPanel_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            TextBox tb = e.Source as TextBox;
            //tb.SelectAll();
            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || e.Key == Key.Tab
                || e.Key == Key.OemMinus || e.Key == Key.Back)
            {
                if (tb.Text.Length == 1)
                {
                    if (e.Key == Key.Back)
                        tb.Text = "0";
                    else if (tb.Text == "0")
                        tb.SelectAll();
                }
                e.Handled = false;
            }
            else
            { e.Handled = true; }
            //else if (string.IsNullOrEmpty( tb.Text)
            //else
            //{
            //    tb.Text = "0";
            //    tb.SelectAll();
            //    e.Handled = true;
            //}
        }

        private void textboxProgram_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox tb = e.Source as TextBox;
            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9))
            {
                changed = true;
                if (tb.Text == "0" && tb.SelectionLength == 0)
                    tb.Text = "";
                if (tb.Text.Length == 2 && tb.SelectionLength == 0)
                    e.Handled = true;
            }
            else if (e.Key == Key.Tab || e.Key == Key.Right || e.Key == Key.Left)
                e.Handled = false;
            else if (e.Key == Key.Back)
            {
                if (tb.Text.Length == 1 || tb.SelectionLength == 2)
                {
                    tb.Text = "0";
                    tb.SelectAll();
                    e.Handled = true;
                }
            }
            else
                e.Handled = true;
        }

        private void check_Checked(object sender, RoutedEventArgs e)
        {
            bool enabled = !(check.IsChecked == true);
            touchSmallProgTB.IsEnabled = enabled;
            alfaSmallProgTB.IsEnabled = enabled;
            //startProgTB.IsEnabled = enabled;
            //sosProgTB.IsEnabled = enabled;
            //touchBigCardTB.IsEnabled = enabled;
            //touchSmallCardTB.IsEnabled = enabled;
            //alfaBigCardTB.IsEnabled = enabled;
            //alfaSmallCardTB.IsEnabled = enabled;
            //startCardTB.IsEnabled = enabled;
            //sosCardTB.IsEnabled = enabled;
        }

        void setNoCards(TextBox tb, string descriptiveText)
        {
            tb.Text = "0";
            tb.FontWeight = FontWeights.ExtraBold;
            tb.Foreground = Brushes.Red;

            TextBlock text = new TextBlock()
            {
                Text = descriptiveText,
                Foreground = Brushes.White,
                Background = Brushes.Red,
                FontFamily = new FontFamily("Verdana"),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextWrapping = TextWrapping.Wrap,
                FontWeight = FontWeights.Bold
            };
            Grid.SetRow(text, Grid.GetRow(tb));
            Grid.SetColumn(text, 1);

            mainGrid.Children.Add(text);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WindowTransactions win = new WindowTransactions(user1, user2);
            win.Tag = this;
            win.Show();

        }

        private void Enable_Click(object sender, RoutedEventArgs e)
        {
            alfaSmallProgTB.IsEnabled = true;
            alfa1MonthProgTB.IsEnabled = true;
            //alfa2MonthProgTB.IsEnabled = true;
            //alfa3MonthProgTB.IsEnabled = true;
        }

        private void BTNprofit_Click(object sender, RoutedEventArgs e)
        {
            new WindowProfit().Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!windowCanBeClosed)
            {
                e.Cancel = true;
                MessageBox.Show("Purchasing of Cards not finished", "wait", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void buttonOldCards_Click(object sender, RoutedEventArgs e)
        {
            List<string> touchBig = new List<string>();
            List<string> touchSmall = new List<string>();
            List<string> touchStart = new List<string>();

            buttonOldCards.IsEnabled = false;
            for (int i = 0; i < 4; i++)
            {
                foreach (MyLogInvoiceResponse invoice in mainUser.Transactions[i].myLogInvoiceResponses)
                {
                    foreach (LogDetail log in invoice.logDetails)
                    {
                        List<string> tempList;
                        if (log.service == "TOUCH")
                        {
                            if (log.denomination == "TOUCH ($12.78)")
                                tempList = touchSmall;
                            else if (log.denomination == "TOUCH ($25.4)")
                                tempList = touchBig;
                            else if (log.denomination == "TOUCH START ($10.26)")
                                tempList = touchStart;
                            else
                                continue;
                            tempList.Add(string.Format("{0}.{1}  {3}-  {2}",
                                (tempList.Count + 1).ToString(),
                                log.formattedSecret,
                                log.date,
                                (tempList.Count < 9) ? " " : ""));
                        }
                    }
                }
            }


            touchBig.Insert(0, "touch25");
            touchBig.Insert(1, "");
            touchStart.Insert(0, "touch start");
            touchStart.Insert(1, "");
            touchSmall.Insert(0, "touch12.5");
            touchSmall.Insert(1, "");
            touchBig.Add("");
            touchBig.AddRange(touchSmall);
            touchBig.Add("");
            touchBig.AddRange(touchStart);

            string path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            File.WriteAllLines(path + "\\old cards.txt", touchBig);
            buttonOldCards.IsEnabled = true;
        }

        public class ItelUser
        {
            //const string balanceUrl = "https://lb.whish.money/itel-service/account/homepagehttps://whish.money/itel-service/sale/accountfinancialdetails";
            const string balanceUrl = "https://lb.whish.money/itel-service/account/homepage";
            const string lostTransactionsUrl = "https://lb.whish.money/itel-service/transaction/lost/new";
            const string purchasevouchersUrl = "https://lb.whish.money/itel-service/sale/voucher";
            const string topupVoucherUrl = "https://lb.whish.money/itel-service/sale/voucherTopup";
            const string soldVouchersUrlold = "https://lb.whish.money/itel-service/transaction/soldItems";
            const string soldVouchersUrl = "https://lb.whish.money/itel-service/transaction/purchases";
            const string costsUrl = "https://lb.whish.money/itel-service/items/services/final";

            string sessionCounterPath;

            public string Token { get; set; }

            public string DeviceId { get; set; }

            public string SessionId { get; set; }

            public string TokenPath { get; set; }
            public string TransactionsPath { get; set; }

            public string SessionCounter { get; set; }

            public bool UniversalAccount = false;

            public List<Transaction> Transactions;

            string servicesVersion = "14723";
            string serverVersion = "27";

            public ItelUser(string path, bool universal)
            {
                SessionCounter = "0";
                TokenPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\" + path;
                Transactions = new List<Transaction>();
                UniversalAccount = universal;
            }

            async public Task GetSessioncounter()
            {

                if (File.Exists(TokenPath))
                {
                    string[] data = File.ReadAllLines(TokenPath);
                    DeviceId = data[0];
                    Token = data[1];
                    SessionId = data[2];
                    //servicesVersion = data[3];

                    TransactionsPath = UniversalAccount ? "universal" : SessionId;

                    if (File.Exists(TransactionsPath))
                    {
                        Transactions = Json.DeserializeListTransactions(File.ReadAllText(TransactionsPath));
                        Transactions.Sort(new TransactionComparer());
                    }
                    else
                    {
                        File.Create(TransactionsPath).Dispose();
                    }
                }
                else
                {
                    SessionCounter = "error";
                    return;
                }
                sessionCounterPath = SessionId + ".session";
                if (File.Exists(sessionCounterPath))
                {
                    SessionCounter = File.ReadAllText(sessionCounterPath);
                }
                else
                {
                    File.WriteAllText(sessionCounterPath, SessionCounter);
                }


                await Task.Delay(200);
                WebClient client = createClient();

                string result = await client.DownloadStringTaskAsync(lostTransactionsUrl);
                //servicesVersion = client.ResponseHeaders["servicesversion"];

                if (result.Contains("update"))
                {
                    SessionCounter = "update";
                    //File.Delete("details.txt");
                }
                else
                {
                    SessionCounter = client.ResponseHeaders["sessionCounter"];
                    servicesVersion = client.ResponseHeaders["servicesversion"];
                    if (SessionCounter == null)
                    {
                        File.WriteAllText(sessionCounterPath, "0");
                        SessionCounter = "restart";
                    }
                    else
                        File.WriteAllText(sessionCounterPath, SessionCounter);


                    //if (!File.Exists("details.txt"))
                    //   File.WriteAllText("details.txt", await client.DownloadStringTaskAsync(costsUrl));
                }
            }

            async public Task UpdateSessionCounter()
            {
                WebClient client = createClient();

                string result = await client.DownloadStringTaskAsync(lostTransactionsUrl);
                if (result.Contains("update"))
                {
                    SessionCounter = "update";
                }
                else
                {
                    SessionCounter = client.ResponseHeaders["sessionCounter"];
                    File.WriteAllText(sessionCounterPath, SessionCounter);
                }
            }

            async public Task<double> GetBalance()
            {
                string post = string.Format("{{\"serverVersion\":{0},\"servicesVersion\":{1}}}", serverVersion, servicesVersion);
                WebClient client = createClient();
                client.Headers.Add("Content-Type", "application/json;charset=UTF-8");
                string result = await client.UploadStringTaskAsync(balanceUrl, post);

                RootObject<BalanceDetails> root = Json.Desirialize<BalanceDetails>(result);
                string stringBalance = Regex.Match(root.data.balance, "My Money: (.*) LBP").Groups[1].Value;
                int intBalance = int.Parse(stringBalance, NumberStyles.AllowThousands);
                return intBalance / 1515.0;
            }

            async public Task<List<Voucher>> PurchaseVouchers(string dominationID, string count, string price)
            {
                string post = string.Format("{{\"denominationId\":{0},\"numberOfItems\":{1},\"price\":{2}}}", dominationID, count, price);
                WebClient client = createClient();
                client.Headers.Add("Content-Type", "application/json;charset=UTF-8");
                string result = await client.UploadStringTaskAsync(purchasevouchersUrl, post);

                //string result = File.ReadAllText(@"C:\Users\SK\Documents\Visual Studio 2015\Projects\TestConsole\TestConsole\bin\Debug\Raw.txt");

                SessionCounter = client.ResponseHeaders["sessionCounter"];
                if (!string.IsNullOrWhiteSpace(SessionCounter))
                    File.WriteAllText(sessionCounterPath, SessionCounter);
                RootObject<SalesData> root = Json.Desirialize<SalesData>(result);
                return root.data.Vouchers;
            }

            async public Task GetTransactions(DateTime transactionDate, bool updateToday = false)
            {
                int index = -1;
                bool today = false, newday = false;
                if (Transactions.Count == 0)
                {
                    newday = true;
                }
                else
                {
                    DateTime firstTransactionDate = DateTime.Parse(Transactions[0].date);
                    today = DateTime.Today.ToString("yyyy-MM-dd") == firstTransactionDate.ToString("yyyy-MM-dd")
                            && DateTime.Today.ToString("yyyy-MM-dd") == transactionDate.ToString("yyyy-MM-dd");
                    newday = transactionDate.Day > firstTransactionDate.Day && transactionDate > firstTransactionDate;
                }

                if (today)
                {
                    if (!updateToday) return;
                    if (Transactions.Count != 0)
                        Transactions.RemoveAt(0);
                }
                else if (!newday)
                {
                    for (int i = 0; i < Transactions.Count; i++)
                    {
                        DateTime d = DateTime.Parse(Transactions[i].date);
                        if (d.ToString("yyyy-MM-dd") == transactionDate.ToString("yyyy-MM-dd"))
                            if (d.TimeOfDay.TotalSeconds != 0)
                            {
                                index = i;
                                break;
                            }
                            else
                                return;
                    }
                }

                await Task.Delay(200);

                string post = string.Format("{{\"account\":{0},\"day\":{1},\"month\":{2},\"year\":{3}}}", (UniversalAccount ? "true" : "false"), transactionDate.Day, transactionDate.Month, transactionDate.Year);

                WebClient client = createClient();
                client.Headers.Add("Content-Type", "application/json;charset=UTF-8");
                string result = await client.UploadStringTaskAsync(soldVouchersUrl, post);

                RootTransaction root = RootTransaction.Desirialize(result);

                Transaction transaction = root.data.details.Count == 0 ?
                    new Transaction() { date = transactionDate.ToString("yyyy-MM-dd"), myLogInvoiceResponses = new List<MyLogInvoiceResponse>() }
                    : root.data.details[0];

                if (newday || (today && updateToday))
                {
                    transaction.date = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                    Transactions.Insert(0, transaction);
                }
                else if (index != -1)
                {
                    Transactions[index] = transaction;
                }
                else
                {
                    Transactions.Add(transaction);
                }
            }

            public class TransactionComparer : IComparer<Transaction>
            {
                public int Compare(Transaction x, Transaction y)
                {
                    return y.date.CompareTo(x.date);
                }
            }

            public void SaveTransactions()
            {
                Transactions.Sort(new TransactionComparer());
                Json.SerializeListTransactions(Transactions, TransactionsPath);
            }

            private WebClient createClient()
            {
                WebClient client = new WebClient();
                client.Headers.Add("language", "en");
                client.Headers.Add("itelVersion", MainWindow.ItelVer);
                client.Headers.Add("token", Token);
                client.Headers.Add("sessionId", SessionId);
                client.Headers.Add("deviceId", DeviceId);
                client.Headers.Add("sessionCounter", SessionCounter);
                client.Headers.Add("locale", "en");
                client.Headers.Add("operatingSystem", "1");

                return client;
            }
        }
    }



}
