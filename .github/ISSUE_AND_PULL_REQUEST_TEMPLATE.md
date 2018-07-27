{{if .IsIssue}}
<!-- This is a generic template and may not be applicable in all cases -->
<!-- Try to follow it when possible -->
### Description of the Issue
<!-- Provide a detailed description of the issue -->

### Steps to Reproduce the Issue
<!-- Set of steps to reproduce this issue -->
1. 
2. 
3. 

### Expected Behavior
<!-- What did you expect to happen -->

### Actual Behavior
<!-- What actually happend -->

### Debug Information
<!-- Please specify the version you are using -->

<!-- Screen-shots and other information are welcome! -->

{{else if .IsPullRequest}}
<!-- This is a generic template and may not be applicable in all cases -->
<!-- Try to follow it when possible -->
### Description of the pull request
<!-- Provide a detailed description of what you did and what is the purpose of this PR -->

### Check List
<!-- Check the items that are true -->

- [ ] I tested this very carefully
- [ ] I added an automated test
- [ ] I tried to blend into the current coding style

{{end}}