using static MonopolyGalaktyczneFull.src.Planeta;
using System.Text.RegularExpressions;
using System;
using Font = System.Drawing.Font;
using MonopolyGalaktyczneFull.src;
using MonopolyGalaktyczneFull.containers;

namespace MonopolyGalaktyczneFull
{
    public partial class GameForm : Form
    {
        public Gra gra;
        private Panel mainPanel;
        private TableLayoutPanel rightPanel;
        private GameGrid monopolyGrid;
        private List<Label> playerLabels;
        private List<Label> structureLabels;
        
        private TableLayoutPanel actionPanel;
        private List<Button> actionButtons;
        private Label actionText;

        private TableLayoutPanel playerInfoPanel;
        private Label playerInfo;
        private List<Label> playerInfoLabels;

        private List<TextBox> playerNickTextBoxes;

        private void startGame(object sender, EventArgs e, int playerCount)
        {
            gra.dodajGraczy(playerCount);
            string[] nicknames = new string[playerCount];
            for (int i = 0; i < playerNickTextBoxes.Count; i++)
            {
                nicknames[i] = playerNickTextBoxes[i].Text;
                playerLabels[0].Text += nicknames[i] + Environment.NewLine;
            }
            gra.stworzGraczy(nicknames);
            gra.start();

        }

        public void KillPlayer(Gracz gracz)
        {
            int obecnePoleIndex = gracz.obecnePole.index;
            gra.usunGracza(gracz);

            Label labelDoUsuniecia = playerInfoLabels[gra.turaGracza];
            playerInfoPanel.Controls.Remove(labelDoUsuniecia);
            playerInfoLabels.Remove(labelDoUsuniecia);

            playerLabels[obecnePoleIndex].Text = "";

            for(int i = 0; i < structureLabels.Count; i++)
            {
                if (structureLabels[i].Text.Contains(gracz.nick))
                {
                    structureLabels[i].Text = "";
                    gra.plansza.pola[i].planeta.reset();
                }
            }

            for (int i = 0; i < gra.gracze.Count; i++)
            {
                if (gra.gracze[i].obecnePole.index == obecnePoleIndex)
                    playerLabels[obecnePoleIndex].Text += gra.gracze[i].nick + Environment.NewLine;
            }
        }

        public void UpdatePlayerInfo(Gracz obecnyGracz)
        {
            for(int i = 0; i < gra.gracze.Count; i++)
            {
                Gracz gracz = gra.gracze[i];
                string info = $"Gracz {gracz.nick}\n" +
                $"{gracz.kasa} kredytów galaktycznych\n" +
                $"{gracz.iloscObronPrzedPiratami}x karta obrony przed piratami\n" +
                $"{gracz.iloscBiletowGalaktycznych}x bilet galaktyczny\n";

                if (gracz.blokadaKolejki > 0)
                    info += "Zablokowany\n\n";
                else
                    info += "\n";

                playerInfoLabels[i].Text = info;

                if (gracz == obecnyGracz)
                    playerInfoLabels[i].Font = new Font("Arial", 11, FontStyle.Bold);
                else
                    playerInfoLabels[i].Font = new Font("Arial", 11, FontStyle.Regular);
            }
        }
        
        public GameForm()
        {
            gra = new Gra();
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.WindowState = FormWindowState.Maximized;
            this.StartPosition = FormStartPosition.CenterScreen;

            CreateMainPanel();
            CreateRightPanel();
            CreateMonopolyGrid();

            ComposeLayout();
            SetupEvents();
        }

        private void CreateMainPanel()
        {
            mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };
        }

        public void CreatePlayerInfo(int playerCount)
        {
            playerInfoLabels = new List<Label>(playerCount);
      
            
            for (int i = 0; i < playerCount; i++)
            {
                playerInfoPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100 / playerCount));

                playerInfo = new Label
                {
                    Dock = DockStyle.Fill,
                    Font = new Font("Arial", 11),
                    TextAlign = ContentAlignment.TopCenter,
                    AutoSize = false,
                    AutoEllipsis = false,
                    MaximumSize = new Size(0, 0),
                    Padding = new Padding(10)
                };
                playerInfoLabels.Add(playerInfo);
                playerInfoPanel.Controls.Add(playerInfo);
            }

        }

        public void HideAddPlayerSection()
        {
            playerInfoPanel.Controls.Clear();
        }

        void CreateAddPlayerSection(int playerCount)
        {
            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 6,
                AutoSize = true
            };

            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40)); // Tytuł
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40)); // TextBox 1
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40)); // TextBox 2
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40)); // TextBox 3
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40)); // TextBox 4
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); // Zatwierdź

            var title = new Label
            {
                Text = "Podaj dane graczy",
                Font = new Font("Arial", 14, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };
            layout.Controls.Add(title, 0, 0);

            //pola na nicki graczy
            playerNickTextBoxes = new List<TextBox>(playerCount);
            for (int i = 0; i < playerCount; i++)
            {
                var textBox = new TextBox
                {
                    PlaceholderText = $"Gracz {i + 1}",
                    Dock = DockStyle.Fill,
                    Margin = new Padding(10)
                };
                playerNickTextBoxes.Add(textBox);
                layout.Controls.Add(textBox, 0, i + 1);
            }

            // Dodaj przycisk zatwierdzania
            var confirmButton = new Button
            {
                Text = "Zatwierdź",
                Dock = DockStyle.Top,
                Height = 40,
                Margin = new Padding(10)
            };
            confirmButton.Click += (sender, e) => startGame(sender, e, playerCount);
            layout.Controls.Add(confirmButton, 0, 5);

            playerInfoPanel.Controls.Add(layout);
        }

        private void CreateInfoPanel()
        {
            playerInfoPanel = new TableLayoutPanel
            {
                BackColor = Color.LightCoral,
                Dock = DockStyle.Fill,
            };

            CreateAddPlayerSection(4);
        }

        void CreateActionPanel()
        {
            actionPanel = new TableLayoutPanel
            {
                BackColor = Color.LightGreen,
                Dock = DockStyle.Fill,
                Padding = new Padding(10),
                RowCount = 2,
                ColumnCount = 1,
                AutoSize = true
            };

            actionPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10));
            actionPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 70));

            rightPanel.Controls.Add(actionPanel);
        }

        public void UpdateActionPanel(string actionText, params string[] buttonsText)
        {
            this.actionText = new Label
            {
                AutoSize = false,
                Text = actionText,
                Font = new Font("Arial", 12),
                TextAlign = ContentAlignment.TopCenter,
                MaximumSize = new Size(0, 0),
                AutoEllipsis = false,
                Padding = new Padding(5),
                Dock = DockStyle.Fill,
            };

            actionPanel.Controls.Clear();
            actionPanel.Controls.Add(this.actionText);

            var actionButtonsPanel = new Panel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(5),
            };

            actionButtons = new List<Button>();
            for (int i = buttonsText.Count() - 1; i >= 0; i--)
            {
                var text = buttonsText[i];
                var button = new Button
                {
                    Text = text,
                    Dock = DockStyle.Top,
                    Font = new Font("Arial", 11),
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    UseCompatibleTextRendering = true,
                    Padding = new Padding(10)
                };

                actionButtons.Insert(0, button);
                actionButtonsPanel.Controls.Add(button);
            }

            actionPanel.Controls.Add(actionButtonsPanel);
        }

        private void CreateRightPanel()
        {
            rightPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Right,
                BackColor = Color.LightGray,
                ColumnCount = 1,
                RowCount = 2,
                Width = this.ClientSize.Width / 4,
                Padding = new Padding(10)
            };

            rightPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            rightPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50));

            CreateInfoPanel();
            CreateActionPanel();

            rightPanel.Controls.Add(playerInfoPanel, 0, 0);
            rightPanel.Controls.Add(actionPanel, 0, 1);
        }

        public void rozbuduj(Pole pole, Gracz gracz)
        {
            int cena;
            if(pole.planeta.typ == typKolonii.Posterunek)
                cena = pole.planeta.cena + pole.planeta.bazaCzynszu;
            else
                cena = pole.planeta.cena;

            if (gracz.kasa < cena)
            {
                MessageBox.Show("Nie masz wystarczająco kredytów galaktycznych");
                return;
            }

            pole.planeta.cena = cena;
            gracz.kasa -= cena;
            pole.planeta.czynsz += pole.planeta.bazaCzynszu;

            if (pole.planeta.typ == typKolonii.Kopalnia)
                gracz.zyskKopalnie += pole.planeta.bazaCzynszu / 2;
            else if (pole.planeta.typ == typKolonii.Farma)
                gracz.zyskFarma += pole.planeta.bazaCzynszu / 4;

            UpdateStructureName(pole, gracz);
        }

        void UpdateStructureName(Pole pole, Gracz gracz)
        {
            string structureName = pole.planeta.nazwaAglomeracji(pole.planeta.poziom);
            structureLabels[pole.index].Text = $"{gracz.nick}: {structureName}";
        }

        public void RozbudujAction(Pole pole, Gracz gracz)
        {
            int cenaStoczni = pole.planeta.cena * 3;
            if (gracz.czyPosiadaWszystkieWUkladzie(gracz.obecnePole.index) && gracz.kasa >= cenaStoczni)
            {
                UpdateActionPanel("Posiadasz wszystkie planety w tym układzie," +
                    $" czy chcesz zbudować stocznie galaktyczną za {cenaStoczni} kredytów galaktycznych?", "Tak", "Nie");
                actionButtons[0].Click += (s, e) =>
                {
                    if (gracz.kasa >= cenaStoczni)
                    {
                        structureLabels[gracz.obecnePole.index].Text = "Stocznia Galaktyczna";
                        gracz.kasa -= cenaStoczni;
                        gracz.zyskStocznia += pole.planeta.bazaCzynszu * 2;
                    }
                    else
                    {
                        Console.WriteLine("Nie masz wystarczająco kredytów galaktycznych");
                    }
                };
                actionButtons[1].Click += (s, e) =>
                {
                    gra.nastepnaTura();
                };
            }
            else if (pole.planeta.poziom < pole.planeta.maxPoziom)
            {
                UpdateActionPanel($"Czy chcesz rozbudować planetę {pole.planeta.nazwa}?", "Tak", "Nie");
                gra.gameForm.actionButtons[0].Click += (s, e) =>
                {
                    rozbuduj(pole, gracz);
                    gra.nastepnaTura();
                };
                gra.gameForm.actionButtons[1].Click += (s, e) =>
                {
                    gra.nastepnaTura();
                };
            }
            else
            {
                UpdateActionPanel($"Nie nie mozesz zrobic", "Zakończ turę");
                gra.gameForm.actionButtons[0].Click += (s, e) =>
                {
                    gra.nastepnaTura();
                };
            }    
        }

        public void WybudujAction(Pole pole, Gracz gracz)
        {
            gra.gameForm.UpdateActionPanel($"Czy chcesz osiedlic się na planecie {pole.planeta.nazwa} za " +
                        $"{pole.planeta.cena} kredytów galaktycznych?", "Tak", "Nie");

            gra.gameForm.actionButtons[0].Click += (s, e) =>
            {
                if (gracz.kasa >= pole.planeta.cena)
                {
                    pole.planeta.osiedl(gracz);
                    gra.gameForm.UpdatePlayerInfo(gracz);
                    UpdateStructureName(pole, gracz);
                }
                else
                {
                    MessageBox.Show("Nie masz wystarczającej ilości kredytów galaktycznych.");
                }
                gra.nastepnaTura();
            };

            gra.gameForm.actionButtons[1].Click += (s, e) =>
            {
                gra.nastepnaTura();
            };
        }

        public void RzucKostkaAction(int pominieteRuchyPodRzad)
        {
            if (pominieteRuchyPodRzad < 2)
            {
                UpdateActionPanel("Co robisz?", "Rzuć kostką", "Pomiń ruch");
                actionButtons[1].Click += (s, e) =>
                {
                    gra.gracze[gra.turaGracza].pominRuch();
                };
            }
            else
            {
                UpdateActionPanel("Co robisz?", "Rzuć kostką");
            }

            actionButtons[0].Click += (s, e) =>
            {
                gra.gracze[gra.turaGracza].rzucKostka();
            };
        }

        public void KolejAction(Gracz gracz)
        {
            if (gracz.iloscBiletowGalaktycznych == 0)
            {
                UpdateActionPanel("Nie posiadasz biletów galaktycznych", "Zakończ turę");
                actionButtons[0].Click += (s, e) =>
                {
                    gra.nastepnaTura();
                };
            }
            else
            {
                UpdateActionPanel("Do jakiego układu chcesz się udać?", "Układ Słoneczny",
                    "Kepler-31", "Zeta Reticuli", "Varkorath", "Zostań na obecnym polu");

                actionButtons[0].Click += (s, e) =>
                {
                    gra.gracze[gra.turaGracza].obecnePole = gra.plansza.pola[3];
                    gra.gameForm.UpdatePlayerPositions(gracz.obecnePole.index, 3);
                    gra.nastepnaTura();
                };
                actionButtons[1].Click += (s, e) =>
                {
                    gra.gracze[gra.turaGracza].obecnePole = gra.plansza.pola[9];
                    gra.gameForm.UpdatePlayerPositions(gracz.obecnePole.index, 9);
                    gra.nastepnaTura();
                };
                actionButtons[2].Click += (s, e) =>
                {
                    gra.gracze[gra.turaGracza].obecnePole = gra.plansza.pola[15];
                    gra.gameForm.UpdatePlayerPositions(gracz.obecnePole.index, 15);
                    gra.nastepnaTura();
                };
                actionButtons[3].Click += (s, e) =>
                {
                    gra.gracze[gra.turaGracza].obecnePole = gra.plansza.pola[21];
                    gra.gameForm.UpdatePlayerPositions(gracz.obecnePole.index, 21);
                    gra.nastepnaTura();

                };
                actionButtons[4].Click += (s, e) =>
                {
                    gra.nastepnaTura();
                };
            }
        }

        public void RozbudujPortAction(Pole pole, Gracz gracz)
        {
            UpdateActionPanel("Czy chcesz rozbudować port kosmiczny?", "Tak", "Nie");
            actionButtons[0].Click += (s, e) =>
            {
                int cenaPosterunek = pole.planeta.cena + pole.planeta.bazaCzynszu;
                int cenaKopalnia = pole.planeta.cena * 2;
                int cenaFarma = pole.planeta.cena;
                UpdateActionPanel("Co chcesz wybudować na tej planecie?", $"Posterunek za {cenaPosterunek} kredytów",
                    $"Kopanię za {cenaKopalnia} kredytów", $"Farmę żywności za {cenaFarma} kredytów", "Nic");
                    
                actionButtons[0].Click += (s, e) =>
                {
                    pole.planeta.wybuduj(Planeta.typKolonii.Posterunek, gracz);
                    UpdateStructureName(pole, gracz);
                    gra.nastepnaTura();
                };
                actionButtons[1].Click += (s, e) =>
                {
                    pole.planeta.wybuduj(Planeta.typKolonii.Kopalnia, gracz);
                    UpdateStructureName(pole, gracz);
                    gra.nastepnaTura();
                };
                actionButtons[2].Click += (s, e) =>
                {
                    pole.planeta.wybuduj(Planeta.typKolonii.Farma, gracz);
                    UpdateStructureName(pole, gracz);
                    gra.nastepnaTura();
                };
                actionButtons[3].Click += (s, e) =>
                {
                    gra.nastepnaTura();
                };
            };
            actionButtons[1].Click += (s, e) =>
            {
                gra.nastepnaTura();
            };
        }

        public void CzynszAction(Pole pole, Gracz gracz)
        {
            UpdateActionPanel($"Płacisz czynsz graczowi {pole.planeta.wlasciciel.nick} w wysokości " +
                $"{pole.planeta.czynsz} kredytów galaktycznych", "Zakończ turę");

            gracz.kasa -= pole.planeta.czynsz;
            pole.planeta.wlasciciel.kasa += pole.planeta.czynsz;

            actionButtons[0].Click += (s, e) =>
            {
                gra.nastepnaTura();
            };  
        }

        public void OsobliwoscAction(Gracz gracz)
        {
            var random = new Random();
            int los;

            if (gracz.zyskStocznia > 0)
                los = random.Next(0, 7);
            else
                los = random.Next(1, 7);

            if (los == 0)
            {
                gra.gameForm.UpdateActionPanel("Awaria stoczni galaktycznej",
                    "Zapłać 2000 kredytów galaktycznych", "Zniszcz stocznię");

                actionButtons[0].Click += (s, e) =>
                {
                    if (gracz.kasa > 2000)
                        gracz.kasa -= 2000;
                    else
                    {
                        MessageBox.Show("Nie masz wystarczająco kredytów galaktycznych, twoja stocznia jest zniszczona");
                        gracz.zyskStocznia -= 500;
                    }
                };
            }
            if (los == 1)
            {
                gra.gameForm.UpdateActionPanel("Znalazłeś bilet galaktyczny", "Zakończ turę");
                actionButtons[0].Click += (s, e) =>
                {
                    gra.nastepnaTura();
                };
                gracz.iloscBiletowGalaktycznych++;
            }
            else if (los == 2)
            {
                gra.gameForm.UpdateActionPanel("Znalazłeś kartę obrony przed piratami", "Zakończ turę");
                actionButtons[0].Click += (s, e) =>
                {
                    gra.nastepnaTura();
                };
                gracz.iloscObronPrzedPiratami++;
            }
            else if (los == 3)
            {
                AtakPiratow(gracz);
            }
            else if (los == 4)
            {
                gra.gameForm.UpdateActionPanel("Wygrałeś w galaktycznej loterii 500 kredytow", "Zakończ turę");
                actionButtons[0].Click += (s, e) =>
                {
                    gra.nastepnaTura();
                };
                gracz.kasa += 500;
            }
            else if (los == 5)
            {
                gra.gameForm.UpdateActionPanel("Silniki twojego statku uległy awarii\n" +
                    "Tracisz 250 kredytów i 1 kolejkę", "Zakończ turę");
                actionButtons[0].Click += (s, e) =>
                {
                    gra.nastepnaTura();
                };
                gracz.kasa -= 250;
                gracz.blokadaKolejki = 1;
            }
            else if (los == 6)
            {
                int podatek = (int)gracz.podatekImperatora();
                gra.gameForm.UpdateActionPanel($"Imperator zarządził pobranie jednorazowego " +
                    $"podatku 10% od nieruchomości, który wynosi {podatek} kredytów galaktycznych", "Zakończ turę");
                actionButtons[0].Click += (s, e) =>
                {
                    gra.nastepnaTura();
                };
                gracz.kasa -= podatek;
            }
        }

        public void AtakPiratow(Gracz gracz)
        {
            UpdateActionPanel("Piraci cię atakują!", "Użyj karty obrony przed piratami", 
                "Zapłać okup 500 kredytów galaktycznych", "Strać 2 kolejki");

            actionButtons[0].Click += (s, e) =>
            {
                if (gracz.iloscObronPrzedPiratami > 0)
                {
                    gracz.iloscObronPrzedPiratami--;   
                }
                else
                {
                    MessageBox.Show("Nie masz karty obrony przed piratami, tracisz 2 kolejki");
                }
                gra.nastepnaTura();
            };

            actionButtons[1].Click += (s, e) =>
            {
                if (gracz.kasa >= 500)
                {
                    gracz.kasa -= 500;
                }
                else
                {
                    MessageBox.Show("Nie masz wystarczającej ilości kredytów galaktycznych");
                }
                gra.nastepnaTura();
            };

            actionButtons[2].Click += (s, e) =>
            {
                gracz.blokadaKolejki = 2;
                gra.nastepnaTura();
            };
        }

        public void GraczZablokowany()
        {
            gra.gracze[gra.turaGracza].blokadaKolejki--;
            UpdateActionPanel("Jesteś zablokowany", "Zakończ turę");

            actionButtons[0].Click += (s, e) =>
            {
                gra.nastepnaTura();
            };
        }

        private void GameForm_Click(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public void UpdatePlayerPositions(int oldPlanet, int newPlanet)
        {
            playerLabels[oldPlanet].Text = "";
            for (int i = 0; i < gra.gracze.Count; i++)
            {
                if (gra.gracze[i].obecnePole.index == oldPlanet)
                    playerLabels[oldPlanet].Text += gra.gracze[i].nick + Environment.NewLine;
            }

            playerLabels[newPlanet].Text = "";
            for (int i = 0; i < gra.gracze.Count; i++)
            {
                if (gra.gracze[i].obecnePole.index == newPlanet)
                    playerLabels[newPlanet].Text += gra.gracze[i].nick + Environment.NewLine;
            }
        }

        void CreateMonopolyGrid()
        {
            Dictionary<int, int> indexToGridPos = new Dictionary<int, int>
            {
                [0] = 0, [1] = 1, [2] = 2, [3] = 3,
                [4] = 4,
                [5] = 5,
                [6] = 6,
                [7] = 23,
                [8] = 7,
                [9] = 22,
                [10] = 8,
                [11] = 21,
                [12] = 9,
                [13] = 20,
                [14] = 10,
                [15] = 19,
                [16] = 11,
                [17] = 18,
                [18] = 17,
                [19] = 16,
                [20] = 15,
                [21] = 14,
                [22] = 13,
                [23] = 12
            };
            monopolyGrid = new GameGrid()
            {
                Dock = DockStyle.Fill,
                BackColor = Color.LightGray,
            };

            playerLabels = new List<Label>(24);
            structureLabels = new List<Label>(24);
            for (int i = 0; i < 24; i++)
            {
                playerLabels.Add(new Label());
                structureLabels.Add(new Label());
            }

            for (int i = 0; i < 24; i++)
            {
                var panel = new TableLayoutPanel
                {
                    BorderStyle = BorderStyle.FixedSingle,
                    Dock = DockStyle.Fill,
                    Padding = new Padding(2),
                    RowCount = 3,
                    ColumnCount = 1
                };

                panel.RowStyles.Add(new RowStyle(SizeType.Percent, 20)); // planeta
                panel.RowStyles.Add(new RowStyle(SizeType.Percent, 60)); // gracz
                panel.RowStyles.Add(new RowStyle(SizeType.Percent, 20));

                string nazwaPola;
                if (gra.plansza.pola[indexToGridPos[i]].typPola == Pole.TypPola.Planeta)
                {
                    nazwaPola = gra.plansza.pola[indexToGridPos[i]].planeta.nazwa;
                }
                else
                {
                    nazwaPola = gra.plansza.pola[indexToGridPos[i]].typPola.ToString();
                }

                var planetLabel = new Label
                {
                    Text = nazwaPola,
                    Dock = DockStyle.Top,
                    TextAlign = ContentAlignment.TopCenter,
                    Font = new Font("Arial", 10, FontStyle.Bold),
                    AutoSize = true
                };

                var nickLabel = new Label
                {
                    Text = "",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.TopCenter,
                    Font = new Font("Arial", 9),
                    AutoSize = false
                };

                var structureLabel = new Label
                {
                    Text = "",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.TopCenter,
                    Font = new Font("Arial", 9, FontStyle.Bold),
                    AutoSize = true
                };

                panel.Controls.Add(planetLabel);
                panel.Controls.Add(nickLabel);
                panel.Controls.Add(structureLabel);
                playerLabels[indexToGridPos[i]] = nickLabel;
                structureLabels[indexToGridPos[i]] = structureLabel;

                monopolyGrid.Controls.Add(panel);
            }

            var centerButton = new Panel
            {
                Text = "ŚRODEK",
                Font = new Font("Arial", 24, FontStyle.Bold),
                BackColor = Color.LightBlue,
                BorderStyle = BorderStyle.FixedSingle
            };

            monopolyGrid.Controls.Add(centerButton);
            monopolyGrid.CenterControl = centerButton;
        }

        void ComposeLayout()
        {
            mainPanel.Controls.Add(monopolyGrid);
            mainPanel.Controls.Add(rightPanel);

            this.Controls.Add(mainPanel);
        }  

        private void SetupEvents()
        {
            this.Resize += (s, e) =>
            {
                rightPanel.Width = this.ClientSize.Width / 4;
            };
            rightPanel.Width = this.ClientSize.Width / 4;
        }    
    }
}
