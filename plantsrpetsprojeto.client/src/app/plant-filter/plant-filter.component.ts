import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { trigger, transition, style, animate } from '@angular/animations';
import { PlantInfo } from '../models/plant-info';

@Component({
  selector: 'app-plant-filter',
  standalone: false,
  templateUrl: './plant-filter.component.html',
  styleUrls: ['./plant-filter.component.css'],
  animations: [
    trigger('fadeInOut', [
      transition(':enter', [
        style({ opacity: 0 }),
        animate('300ms ease-out', style({ opacity: 1 }))
      ]),
      transition(':leave', [
        animate('200ms ease-in', style({ opacity: 0 }))
      ])
    ])
  ]
})

export class PlantFilterComponent implements OnInit {
  @Input() plants!: PlantInfo[]; // Add this line
  @Input() isOpen = false;
  @Output() closed = new EventEmitter<void>();
  @Output() filtersChange = new EventEmitter<any>();

  filterForm!: FormGroup;

  sunlightOptions = ['Full Sun', 'Part Shade', 'Part Sun/Part Shade', 'Full Shade', 'Filtered Shade'];
  wateringOptions = ['Frequent', 'Average', 'Minimal'];
  cycleOptions = ['Perennial', 'Annual', 'Biennial'];
  growthRateOptions = ['High', 'Moderate', 'Low'];
  careLevelOptions = ['Medium', 'Moderate', 'High'];
  maintenanceOptions = ['High', 'Moderate', 'Low'];
  floweringSeasonOptions = ['Spring', 'Summer', 'Fall', 'Winter'];
  harvestSeasonOptions = ['Spring', 'Summer', 'Fall', 'Winter'];

  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  private initializeForm(): void {
    this.filterForm = this.fb.group({
      edible: [''],
      indoor: [''],
      medicinal: [''],
      fruits: [''],
      flowers: [''],
      cuisine: [''],

      sunlight: [''],
      watering: [''],
      cycle: [''],
      growthRate: [''],
      careLevel: [''],
      maintenance: [''],
      floweringSeason: [''],
      harvestSeason: ['']
    });
  }

  closeModal(): void {
    this.isOpen = false;
    this.closed.emit();
  }

  applyFilters(): void {
    this.filtersChange.emit(this.filterForm.value);
    this.closeModal();
  }

  resetFilters(): void {
    this.filterForm.reset({
      edible: '',
      indoor: '',
      medicinal: '',
      fruits: '',
      flowers: '',
      cuisine: '',
      sunlight: '',
      watering: '',
      cycle: '',
      growthRate: '',
      careLevel: '',
      maintenance: '',
      floweringSeason: '',
      harvestSeason: ''
    });
    this.filtersChange.emit({});
  }
}
