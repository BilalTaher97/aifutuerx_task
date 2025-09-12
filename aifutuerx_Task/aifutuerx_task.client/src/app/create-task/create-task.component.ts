import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { TaskService } from '../Service/task.service';
import { TaskItem } from '../app.module';
import { jwtDecode } from 'jwt-decode';

@Component({
  selector: 'app-create-task',
  templateUrl: './create-task.component.html',
  styleUrls: ['./create-task.component.css']
})
export class CreateTaskComponent {
  newTask: Partial<TaskItem> = {
    userId: 1,
    title: '',
    description: '',
    statusId: 1
  };


  
  constructor(private taskService: TaskService, private router: Router) { }

  getCustomerIdFromToken(): number | null {
    const token = localStorage.getItem('jwtToken');

    if (token) {
      try {
        const decodedToken: any = jwtDecode(token);


        console.log('Decoded Token:', JSON.stringify(decodedToken, null, 2));


        if (decodedToken.nameid) {
          return +decodedToken.nameid;
        }


        if (decodedToken.sub) {
          return +decodedToken.sub;
        }

        return null;

      } catch (error) {
        console.error('Error decoding token:', error);
        return null;
      }
    }
    return null;
  }

  addTask() {
    let user_Id: any;

    user_Id = this.getCustomerIdFromToken();
    this.newTask.userId = user_Id;

    console.log('-------------------------------');
    console.log(this.newTask);
    console.log('-------------------------------');


    this.taskService.createTask(this.newTask).subscribe({
      next: () => {
        this.router.navigate(['/tasks']);
      },
      error: (err) => {
        console.error('Error creating task:', err);
      }
    });
  }


  //addTask() {
  //  this.taskService.createTask(this.newTask).subscribe({
  //    next: () => {
  //      this.router.navigate(['/tasks']);
  //    },
  //    error: (err) => {
  //      console.error('Error creating task:', err);
  //    }
  //  });
  //}

}
