using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using BillNest.Models;
using BillNest.Data;
using BillNest.ViewModels;

namespace BillNest.Views
{
    public partial class MainWindow : Window
    {
        private MainViewModel _vm;
        private Client _clientSelectionne = null; // Stocke le client pour la modification (Update)

        public MainWindow()
        {
            InitializeComponent();
            _vm = new MainViewModel();
            this.DataContext = _vm;
            ChargerDonnees(); // READ au démarrage
        }

        // --- 1. READ (Charger la liste) ---
        private void ChargerDonnees()
        {
            try
            {
                var db = new DatabaseHelper();
                // On met à jour la liste observable du ViewModel
                _vm.Clients = new ObservableCollection<Client>(db.LireClients());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement : " + ex.Message);
            }
        }

        // --- 2. CREATE & UPDATE (Enregistrer) ---
        private void AjouterClient_Click(object sender, RoutedEventArgs e)
        {
            // Vérification minimum
            if (string.IsNullOrWhiteSpace(TxtNom.Text))
            {
                MessageBox.Show("Veuillez entrer au moins un nom.");
                return;
            }

            var db = new DatabaseHelper();

            if (_clientSelectionne == null)
            {
                // Mode CREATION
                db.AjouterClient(new Client
                {
                    Nom = TxtNom.Text,
                    Prenom = TxtPrenom.Text,
                    Email = TxtEmail.Text,
                    Telephone = TxtTel.Text
                });
            }
            else
            {
                // Mode MODIFICATION
                _clientSelectionne.Nom = TxtNom.Text;
                _clientSelectionne.Prenom = TxtPrenom.Text;
                _clientSelectionne.Email = TxtEmail.Text;
                _clientSelectionne.Telephone = TxtTel.Text;

                db.ModifierClient(_clientSelectionne);
            }

            ChargerDonnees(); // Rafraîchir la liste
            ViderFormulaire(); // Reset les champs
        }

        // --- 3. DELETE (Supprimer) ---
        private void SupprimerClient_Click(object sender, RoutedEventArgs e)
        {
            // On récupère le client lié au bouton via le "Tag" défini dans le XAML
            if (sender is Button btn && btn.Tag is Client clientASupprimer)
            {
                var resultat = MessageBox.Show($"Supprimer {clientASupprimer.Nom} {clientASupprimer.Prenom} ?",
                                              "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (resultat == MessageBoxResult.Yes)
                {
                    new DatabaseHelper().SupprimerClient(clientASupprimer.Id);
                    ChargerDonnees();
                    ViderFormulaire();
                }
            }
        }

        // --- 4. SELECTION (Remplir le formulaire pour modifier) ---
        private void ClientsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ClientsListBox.SelectedItem is Client c)
            {
                _clientSelectionne = c;
                TxtNom.Text = c.Nom;
                TxtPrenom.Text = c.Prenom;
                TxtEmail.Text = c.Email;
                TxtTel.Text = c.Telephone;
            }
        }

        // Méthode utilitaire pour vider les champs
        private void ViderFormulaire()
        {
            TxtNom.Clear();
            TxtPrenom.Clear();
            TxtEmail.Clear();
            TxtTel.Clear();
            _clientSelectionne = null;
            ClientsListBox.SelectedItem = null;
        }
    }
}