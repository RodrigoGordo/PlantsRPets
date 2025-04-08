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

/**
 * Componente responsável por apresentar um formulário de filtros para plantas.
 * Permite ao utilizador filtrar uma lista de plantas com base em diferentes critérios como tipo de rega, exposição solar,
 * estação de colheita, comestibilidade, entre outros.
 */
export class PlantFilterComponent implements OnInit {
  @Input() plants!: PlantInfo[];
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

  /**
   * Construtor com injeção de dependência do FormBuilder para criação de formulários reativos.
   */
  constructor(private fb: FormBuilder) { }

  /**
   * Inicializa o componente ao ser carregado, incluindo a criação do formulário de filtros.
   */
  ngOnInit(): void {
    this.initializeForm();
  }

  /**
   * Inicializa o formulário reativo com os campos e valores padrão (vazios).
   */
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

  /**
   * Fecha o modal de filtros e emite o evento `closed`.
   */
  closeModal(): void {
    this.isOpen = false;
    this.closed.emit();
  }

  /**
   * Aplica os filtros preenchidos e emite os valores do formulário através do evento `filtersChange`.
   * Também fecha o modal após aplicar os filtros.
   */
  applyFilters(): void {
    this.filtersChange.emit(this.filterForm.value);
    this.closeModal();
  }

  /**
   * Reinicia todos os filtros para os valores padrão (vazios) e emite um objeto vazio como filtro aplicado.
   */
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
