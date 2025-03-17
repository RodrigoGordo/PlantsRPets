import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { PlantInfo } from '../models/plant-info';

@Component({
  selector: 'app-plant-filter',
  standalone: false,
  templateUrl: './plant-filter.component.html',
  styleUrls: ['./plant-filter.component.css']
})
export class PlantFilterComponent implements OnChanges {
  @Input() plants: PlantInfo[] = [];
  @Output() filtersChanged = new EventEmitter<any>();

  filterForm: FormGroup;
  filterOptions: { [key: string]: string[] } = {};

  constructor(private fb: FormBuilder) {
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

    this.filterForm.valueChanges.subscribe(values => {
      this.filtersChanged.emit(values);
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['plants'] && this.plants) {
      this.initializeFilterOptions();
    }
  }

  private initializeFilterOptions(): void {
    this.filterOptions = {
      sunlight: this.getUniqueArrayValues('sunlight'),
      watering: this.getUniqueValues('watering'),
      cycle: this.getUniqueValues('cycle'),
      growthRate: this.getUniqueValues('growthRate'),
      careLevel: this.getUniqueValues('careLevel'),
      maintenance: this.getUniqueValues('maintenance'),
      floweringSeason: this.getUniqueValues('floweringSeason'),
      harvestSeason: this.getUniqueValues('harvestSeason')
    };
  }

  private getUniqueValues(field: keyof PlantInfo): string[] {
    return [...new Set(
      this.plants.map(p => p[field]).filter(v => v != undefined && v != null)
    )] as string[];
  }

  private getUniqueArrayValues(field: keyof PlantInfo): string[] {
    return [...new Set(this.plants.flatMap(p => p[field] as string[]))];
  }
}
