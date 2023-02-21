using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Newtonsoft.Json;

namespace Diary
{
    public partial class MainWindow : Window
    {
        private List<Note> notes = new List<Note>();
        private DateTime selectedDate = DateTime.Today;
        private string notesFile = "notes.json";
        private TextBox textTitle;

        public MainWindow()
        {
            InitializeComponent();

            textTitle = new TextBox(); // Initialize textTitle

            LoadNotes();
            PopulateNotesList();
        }

        private void LoadNotes()
        {
            if (File.Exists(notesFile))
            {
                string json = File.ReadAllText(notesFile);
                notes = JsonConvert.DeserializeObject<List<Note>>(json);
            }
        }

        private void PopulateNotesList()
        {
            listNotes.Items.Clear();
            foreach (var note in notes)
            {
                if (note.Date.Date == selectedDate.Date)
                {
                    listNotes.Items.Add(note);
                }
            }
        }

        private void listNotes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Note selectedNote = (Note)listNotes.SelectedItem;
            if (selectedNote != null)
            {
                textTitle.Text = selectedNote.Title; // Use textTitle instead of TitleTextBox
                textDescription.Text = selectedNote.Description;
            }
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            Note note = new Note
            {
                Title = textTitle.Text, // Use textTitle instead of TitleTextBox
                Description = textDescription.Text,
                Date = selectedDate
            };
            notes.Add(note);
            PopulateNotesList();
        }

        private void buttonUpdate_Click(object sender, RoutedEventArgs e)
        {
            Note selectedNote = (Note)listNotes.SelectedItem;
            if (selectedNote != null)
            {
                selectedNote.Title = textTitle.Text; // Use textTitle instead of TitleTextBox
                selectedNote.Description = textDescription.Text;
                PopulateNotesList();
            }
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            Note selectedNote = (Note)listNotes.SelectedItem;
            if (selectedNote != null)
            {
                notes.Remove(selectedNote);
                PopulateNotesList();
            }
        }

        private void datePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedDate = (DateTime)datePicker.SelectedDate;
            PopulateNotesList();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string json = JsonConvert.SerializeObject(notes);
            File.WriteAllText(notesFile, json);
        }
    }
}