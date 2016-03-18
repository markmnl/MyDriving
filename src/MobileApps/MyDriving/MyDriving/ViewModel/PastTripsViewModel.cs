﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using MyDriving.DataObjects;
using MyDriving.Helpers;
using MyDriving.Utils;

using Acr.UserDialogs;
using MvvmHelpers;
using Plugin.DeviceInfo;

namespace MyDriving.ViewModel
{
    public class PastTripsViewModel : ViewModelBase
    {
        public ObservableRangeCollection<Trip> Trips { get; } = new ObservableRangeCollection<Trip>();

        ICommand  loadPastTripsCommand;

        public async Task<bool> ExecuteDeleteTripCommand(Trip trip)
        {
            if (IsBusy)
                return false;

            var progress = UserDialogs.Instance.Loading("Deleting Trip...", show: false, maskType: MaskType.Clear);

            try
            {
                var result = await UserDialogs.Instance.ConfirmAsync($"Are you sure you want to delete trip: {trip.Name}?", "Delete trip?", "Delete", "Cancel");

                if (!result)
                    return false;

                progress?.Show();

                await StoreManager.TripStore.RemoveAsync(trip);

                Trips.Remove(trip);
                Settings.Logout();
            }
            catch (Exception ex)
            {
                Logger.Instance.Report(ex);
            }
            finally
            {
                progress?.Dispose();
            }

           
            return true;
        }

        public ICommand LoadPastTripsCommand =>
        loadPastTripsCommand ?? (loadPastTripsCommand = new RelayCommand(async () => await ExecuteLoadPastTripsCommandAsync())); 

        public async Task ExecuteLoadPastTripsCommandAsync()
        {
            if(IsBusy)
                return;

            var track = Logger.Instance.TrackTime("LoadTrips");
            track?.Start();

			IProgressDialog progressDialog = null;
			
			progressDialog = UserDialogs.Instance.Loading("Loading trips...", maskType: MaskType.Clear);			   

            try 
            {
                IsBusy = true;
                CanLoadMore = true;

                var items = await StoreManager.TripStore.GetItemsAsync(0, 25, true);
                Trips.ReplaceRange(items);

                CanLoadMore = Trips.Count == 25;
            }
            catch (Exception ex) 
            {
                Logger.Instance.Report(ex);
            } 
            finally 
            {
                track?.Stop();
                IsBusy = false;

				progressDialog?.Dispose();
            }

            if (Trips.Count == 0)
            {
                UserDialogs.Instance.Alert("Looks like you don't have any trips recorded yet, feel free to start one up.",
                   "No Past Trips", "OK");
            }

        }

        ICommand  loadMorePastTripsCommand;
        public ICommand LoadMorePastTripCommand =>
        loadMorePastTripsCommand ?? (loadMorePastTripsCommand = new RelayCommand(async () => await ExecuteLoadMorePastTripsCommandAsync()));

        public async Task ExecuteLoadMorePastTripsCommandAsync()
        {
            if (IsBusy || !CanLoadMore)
                return;

            var track = Logger.Instance.TrackTime("LoadMoreTrips");
            track?.Start();
            var progress = UserDialogs.Instance.Loading("Loading more trips...", maskType: MaskType.Clear);
            
            try
            {
                IsBusy = true;
                CanLoadMore = true;

                Trips.AddRange(await StoreManager.TripStore.GetItemsAsync(Trips.Count, 25, true));
            }
            catch (Exception ex) 
            {
                Logger.Instance.Report(ex);
            } 
            finally 
            {
                track?.Stop();
                IsBusy = false;
                progress?.Dispose();
            }
        }
    }
}