﻿using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using System.Data;
using System.Windows.Input;
using Tennis.Exceptions;
using Tennis.Helpers;
using Tennis.Interfaces;
using Tennis.Models;

namespace Tennis.Services
{
    public class EventService : Connection, IEventService
    {
        public EventService()
        {
            connectionString = Secret.ConnectionString;
        }
        public EventService(bool test)
        {
            if (test) 
            {
                connectionString = Secret.ConnectionStringTest;
            }
            else
            {
                connectionString = Secret.ConnectionString;
            }

        }

     
        private string getAllString = "SELECT * FROM Events";

        private string getFromIdString = "SELECT * FROM Events\n" +
                                         "WHERE EventID = @EventID";
        private string insertString = "INSERT INTO Events (Title, Description, Cancelled, DateStart, DateEnd, CancellationThreshold )\n" +
                                      "VALUES (@Title, @Description, @Cancelled, @DateStart, @DateEnd, @CancellationThreshold)";

        private string deleteString = "DELETE FROM Events WHERE EventID = @EventID";

        private string updateById = "UPDATE Events SET Title = @Title, Description = @Description, Cancelled = @Cancelled, DateStart = @DateStart, DateEnd = @DateEnd, CancellationThreshold = @CancellationThreshold WHERE EventID = @EventID";

        public bool CreateEvent(Event evt)
        {
            if (evt == null)
            {
                return false;
            }
            else if (evt.EventState == RelativeTime.Past)
            {
                throw new InvalidTimeException("Event kan ikke blive oprettet i fortid");
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                try
                {
                    SqlCommand command = new SqlCommand(insertString, connection);
                    command.Connection.Open();
                    command.Parameters.AddWithValue("@Title", evt.Title);
                    command.Parameters.AddWithValue("@Description", evt.Description);
                    command.Parameters.AddWithValue("@Cancelled", evt.Cancelled);
                    command.Parameters.AddWithValue("@DateStart", evt.EventTime.StartTime);
                    command.Parameters.AddWithValue("@DateEnd", evt.EventTime.EndTime);
                    command.Parameters.AddWithValue("@CancellationThreshold", evt.CancellationThresholdMinutes);
                    return command.ExecuteNonQuery() != 0;
                    
                }
                catch (SqlException ex)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return false;
        }

        public bool CreateEventNoRequirements(Event evt)
        {
            if (evt == null)
            {
                return false;
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                try
                {
                    SqlCommand command = new SqlCommand(insertString, connection);
                    command.Connection.Open();
                    command.Parameters.AddWithValue("@Title", evt.Title);
                    command.Parameters.AddWithValue("@Description", evt.Description);
                    command.Parameters.AddWithValue("@Cancelled", evt.Cancelled);
                    command.Parameters.AddWithValue("@DateStart", evt.EventTime.StartTime);
                    command.Parameters.AddWithValue("@DateEnd", evt.EventTime.EndTime);
                    command.Parameters.AddWithValue("@CancellationThreshold", evt.CancellationThresholdMinutes);
                    return command.ExecuteNonQuery() != 0;

                }
                catch (SqlException ex)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return false;
        }

        public bool DeleteEvent(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(deleteString, connection);
                    command.Parameters.AddWithValue("@EventId", id);
                    if (command.ExecuteNonQuery() == 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                    
                }
                catch (SqlException ex)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return false;
        }

        public bool EditEvent(Event evt, int id)
        {
            if (evt == null)
            {
                return false;
            }
            else if (evt.EventState == RelativeTime.Past)
            {
                throw new InvalidTimeException("Event cannot be edited to be in the past");
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(updateById, connection);
                    command.Parameters.AddWithValue("@EventID", id);
                    command.Parameters.AddWithValue("@Title", evt.Title);
                    command.Parameters.AddWithValue("@Description", evt.Description);
                    command.Parameters.AddWithValue("@Cancelled", evt.Cancelled);
                    command.Parameters.AddWithValue("@DateStart", evt.EventTime.StartTime);
                    command.Parameters.AddWithValue("@DateEnd", evt.EventTime.EndTime);
                    command.Parameters.AddWithValue("@CancellationThreshold", evt.CancellationThresholdMinutes);
                    if (command.ExecuteNonQuery() == 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                    
                    
                }
                catch (SqlException ex)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return false;
        }

        public List<Event> GetAllEvents()
        {
            List<Event> events = new List<Event>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(getAllString, connection);
                    command.Connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        int eventID = reader.GetInt32("EventID");
                        string title = reader.GetString("Title");
                        string description = reader.GetString("Description");
                        bool cancelled = reader.GetBoolean("Cancelled");
                        TimeBetween eventTime = new TimeBetween(reader.GetDateTime("DateStart"), reader.GetDateTime("DateEnd"));
                        int cancellationThreshold = reader.GetInt32("CancellationThreshold");
                        Event @event = new Event(eventID,title, cancellationThreshold, description, eventTime, cancelled);
                        events.Add(@event);
                    }
                    reader.Close();
                }
                catch (SqlException ex)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return events;
        }

        public Event GetEventByNumber(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(getFromIdString, connection);
                    command.Connection.Open();
                    command.Parameters.AddWithValue("@EventID", id);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows && reader.Read())
                    {
                        int eventID = reader.GetInt32("EventID");
                        string title = reader.GetString("Title");
                        string description = reader.GetString("Description");
                        bool cancelled = reader.GetBoolean("Cancelled");
                        TimeBetween eventTime = new TimeBetween(reader.GetDateTime("DateStart"), reader.GetDateTime("DateEnd"));
                        int cancellationThreshold = reader.GetInt32("CancellationThreshold");
                        Event @event = new Event(eventID, title, cancellationThreshold, description, eventTime, cancelled);
                        reader.Close();
                        return @event;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (SqlException ex)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return null;
        }
    }
}
