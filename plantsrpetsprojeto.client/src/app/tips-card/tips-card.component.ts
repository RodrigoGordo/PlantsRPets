import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Tip } from '../models/tip-model';

@Component({
  selector: 'app-tips-card',
  standalone: false,
  templateUrl: './tips-card.component.html',
  styleUrls: ['./tips-card.component.css']
})

/**
 * Componente responsável por exibir um conjunto de dicas de sustentabilidade
 * relacionadas com uma planta específica.
 */
export class TipsCardComponent {
  @Input() plantInfoId!: number;
  @Input() tips: Tip[] = [];
}
