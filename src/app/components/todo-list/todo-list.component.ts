import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AuthService } from '../../services/auth.service'; // Ensure correct import
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Todo } from './todo.model';

@Component({
  selector: 'app-todo',
  templateUrl: './todo-list.component.html',
  styleUrls: ['./todo-list.component.css']
})
export class TodoComponent implements OnInit {
  todos: any[] = [];
  todoForm: FormGroup;
  taskStatuses: any[] = []; // Store retrieved task statuses
  apiUrl = 'http://localhost:7095/api/Todo'; 
  isEditing = false;
  editingTodoId: number | null = null;

  constructor(private http: HttpClient, private fb: FormBuilder, private authService: AuthService) {
    this.todoForm = this.fb.group({
      todoTitle: ['', [Validators.required, Validators.maxLength(255)]],
      todoDescription: ['', [Validators.maxLength(1000)]],
      todoTaskStatusId: [1, Validators.required] // Default status
    });
  }

  ngOnInit(): void {
    this.getTodos();
    this.getTaskStatuses(); 
  }

  getAuthHeaders(): HttpHeaders {
    const token = localStorage.getItem('token'); // Get token from localStorage
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`, // Attach token
      'Content-Type': 'application/json'
    });
  }

  getTodos() {
    this.authService.getTodos().subscribe({
      next: (todos: Todo[]) => {
        this.todos = todos; // ✅ Ensures it's always an array
        console.log('Todos retrieved:', this.todos);
      },
      error: (err) => {
        console.error('Error fetching todos:', err);
      }
    });
  }
  
  getTaskStatuses() {
    this.authService.getTodoTaskStatuses().subscribe({
      next: (statuses) => (this.taskStatuses = statuses),
      error: (err) => console.error('Error fetching task statuses:', err)
    });
  }

  addOrUpdateTodo() {
    if (this.todoForm.valid) {
      const headers = { headers: this.getAuthHeaders() };

      if (this.isEditing && this.editingTodoId) {
        // Update existing todo
        this.http.put(`${this.apiUrl}/${this.editingTodoId}`, this.todoForm.value, headers).subscribe({
          next: () => {
            this.getTodos();
            this.resetForm();
          },
          error: (err) => console.error('Error updating todo:', err),
        });
      } else {
        // Create new todo
        this.http.post(this.apiUrl, this.todoForm.value, headers).subscribe({
          next: () => {
            this.getTodos();
            this.resetForm();
          },
          error: (err) => console.error('Error adding todo:', err),
        });
      }
    }
  }

  editTodo(todo: any) {
    this.isEditing = true;
    this.editingTodoId = todo.todoId;
    this.todoForm.patchValue({
      todoTitle: todo.todoTitle,
      todoDescription: todo.todoDescription,
      todoTaskStatusId: todo.todoTaskStatusId,
    });
  }

  deleteTodo(id: number) {
    if (confirm('Are you sure you want to delete this todo?')) {
      this.http.delete(`${this.apiUrl}/${id}`, { headers: this.getAuthHeaders() }).subscribe({
        next: () => this.getTodos(),
        error: (err) => console.error('Error deleting todo:', err),
      });
    }
  }

  resetForm() {
    this.isEditing = false;
    this.editingTodoId = null;
    this.todoForm.reset();
    this.todoForm.patchValue({ todoTaskStatusId: 1 }); // Reset status to default
  }
}
