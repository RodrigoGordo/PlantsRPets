import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Tip } from '../models/tip-model';

@Component({
  selector: 'app-tips-card',
  standalone: false,
  templateUrl: './tips-card.component.html',
  styleUrls: ['./tips-card.component.css']
})
export class TipsCardComponent {
  @Input() plantInfoId!: number; // Plant ID
  @Input() tips: Tip[] = []; // List of tips
}
