import { Component } from '@angular/core';
import { TaskItem } from '../app.module';
import { TaskService } from '../Service/task.service';
import { jwtDecode } from 'jwt-decode';

import jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';
import { Router } from '@angular/router';

@Component({
  selector: 'app-tasks',
  templateUrl: './tasks.component.html',
  styleUrls: ['./tasks.component.css']
})
export class TasksComponent {
  tasks: TaskItem[] = [];
  filter: string = 'all';
  editingTask: any = null;

  statusMap: any = {
    1: 'Pending',
    2: 'In Progress',
    3: 'Completed'
  };

  constructor(private taskService: TaskService, private router: Router) { }

  ngOnInit(): void {
    this.loadTasks();
  }

  getCustomerIdFromToken(): number | null {
    const token = localStorage.getItem('jwtToken');
    if (token) {
      try {
        const decodedToken: any = jwtDecode(token);
        console.log('Decoded Token:', JSON.stringify(decodedToken, null, 2));
        if (decodedToken.nameid) return +decodedToken.nameid;
        if (decodedToken.sub) return +decodedToken.sub;
        return null;
      } catch (error) {
        console.error('Error decoding token:', error);
        return null;
      }
    }
    return null;
  }

  loadTasks() {

    let userId: any;
     userId = this.getCustomerIdFromToken();
    this.taskService.getTasksByFilter(userId, this.filter).subscribe(data => {
      this.tasks = data;
    });
  }

  setFilter(filter: string) {
    this.filter = filter;
    this.loadTasks();
  }

  deleteTask(id: number) {
    this.taskService.deleteTask(id).subscribe(() => {
      this.tasks = this.tasks.filter(t => t.taskId !== id);
    });
  }

  startEdit(task: TaskItem) {
    if (task.statusId === 3) return;
    this.editingTask = { ...task };
  }

  cancelEdit() {
    this.editingTask = null;
  }

  saveEdit() {
    if (this.editingTask) {
      this.taskService.updateTask(this.editingTask.taskId, this.editingTask).subscribe(updated => {
        const index = this.tasks.findIndex(t => t.taskId === updated.taskId);
        if (index !== -1) {
          this.tasks[index] = {
            ...updated,
            statusName: this.statusMap[updated.statusId]
          };
        }
        this.editingTask = null;
      });
    }
  }

  exportTasksAsPdf() {
    const doc = new jsPDF();

    doc.setFontSize(18);
    doc.text('📋 My Tasks Report', 14, 20);

    autoTable(doc, {
      head: [['#', 'Title', 'Status', 'Description']],
      body: this.tasks.map((t, index) => [
        index + 1,
        t.title,
        this.statusMap[t.statusId] || 'Unknown',
        t.description || ''
      ]),
      startY: 30,
      theme: 'grid',
      styles: { fontSize: 12, cellPadding: 4 },
      headStyles: { fillColor: [79, 70, 229] } 
    });

    doc.save('my-tasks.pdf');
  }


  logout() {
    localStorage.removeItem('token'); 
    this.router.navigate(['/Login']);
  }
}
