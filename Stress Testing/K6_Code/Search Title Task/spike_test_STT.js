import http from "k6/http";
import { sleep, check, group } from "k6";
const token ="Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJFbWFpbCI6IjExMDMwNjAxOUBuY2N1LmVkdS50dyIsIlJvbGUiOiJOQ0NVU3R1ZGVudCIsIlVzZXJJZCI6IjY0Y2QxYTE0NjEwMjgzNjYzNjRlM2YxMiIsImV4cCI6MTcyNjEwNjcyOSwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzI0MyIsImF1ZCI6ImZyb250LWVuZC11cmwifQ.PbS2Gd2gF17Pzr7GZAjbufl0Gyh5vWtR5NWZP3Q0Zt0";
const url = "https://localhost:7243/nccupass/Task/title-search/test/1/0";

export const options = {
  stages: [
    //1st vers: (X: 24% request is okay, the rest has error)
    { duration: '1m', target: 10 }, // Maintain a steady load of 10 VUs for 1 minute.
    { duration: '1m', target: 50 }, // Spike: Increase to 50 VUs for 1 minute.
    { duration: '1m', target: 10 }, // Back to the baseline of 10 VUs for 1 minute.
    { duration: '1m', target: 100 }, // Spike: Increase to 100 VUs for 1 minute.
    { duration: '1m', target: 10 }, // Back to the baseline of 10 VUs for 1 minute.
    { duration: '1m', target: 200 }, // Spike: Increase to 200 VUs for 1 minute.
  ],
};
export default function () {
  const headers = {
    'accept': 'text/plain', // Update the accept header to 'application/json'
    'Authorization': token,
  };
  
  const res = http.get(url, {headers});

  check(res, {
    'Status is 200': (r) => r.status === 200,
  });

  sleep(1);
}