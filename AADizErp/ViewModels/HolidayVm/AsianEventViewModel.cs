﻿using AADizErp;
using AADizErp.Models.Dtos.HrDtos;
using AADizErp.Services.HrServices;
using AADizErp.ViewModels;
using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;

namespace AADizErp.ViewModels.HolidayVm
{
    public partial class AsianEventViewModel : BaseViewModel
    {
        private int pageNumber = 1;
        private int pageSize = 8;
        private int totalCount = 0;

        private readonly OccasionService _occasionService;
        public ObservableRangeCollection<OccasionDto> Occasions { get; private set; } = new();

        public AsianEventViewModel(OccasionService occasionService)
        {
            Title="Event";
            _occasionService = occasionService;
            GetCompanyOccasionDays(pageNumber, pageSize, "event");
        }

        private void GetCompanyOccasionDays(int pageIndex, int showRecord, string tag)
        {
            if (totalCount != 0) totalCount = 0;
            Occasions.Clear();
            IsLoading = true;
            Task.Run(async () =>
            {
                var returnOccasions = await _occasionService.GetCompanyOccasionDays(pageIndex, showRecord, tag);
                if (returnOccasions.Count > 0)
                {
                    App.Current.Dispatcher.Dispatch(() =>
                    {
                        totalCount = returnOccasions.Count;
                        Occasions.ReplaceRange(returnOccasions.Data);
                    });
                    IsLoading = false;
                }
                else
                {
                    IsLoading = false;
                }
            });

        }

        [RelayCommand]
        async Task LoadMoreUcEmployees()
        {
            if (totalCount == Occasions.Count())
            {
                return;
            }
            IsLoading = true;
            pageNumber++;
            var returnOccasions = await _occasionService.GetCompanyOccasionDays(pageNumber, pageSize, "event");
            if (returnOccasions.Count > 0)
            {
                Occasions.AddRange(returnOccasions.Data);
                IsLoading = false;
            }
            else
            {
                IsLoading = false;
            }
        }
    }
}
