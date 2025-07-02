# Test Documentation Directory

This directory contains all test documentation files created as part of the interactive testing workflow defined in [InteractiveTestDocumentation.mdc](mdc:.cursor/rules/InteractiveTestDocumentation.mdc).

## Directory Structure

```
Documentation/tests/
├── README.md (this file)
├── Task_[ID]_Test.md (individual task tests)
├── Subtask_[ID.SubID.Step]_Test.md (subtask tests)
├── PlayerMechanics/ (organized by system)
├── CombatSystem/
├── UI/
└── Integration/
```

## Test File Naming Convention

- **Task Tests**: `Task_[ID]_Test.md` (e.g., `Task_7_Test.md`)
- **Subtask Tests**: `Subtask_[ID.SubID.Step]_Test.md` (e.g., `Subtask_7.1.2_Test.md`)
- **System Tests**: `[SystemName]_[TestType]_Test.md` (e.g., `CombatSystem_Integration_Test.md`)

## Test Types

### Manual Tests
Interactive tests that require user participation and observation, guided step-by-step by Claude.

### Integration Tests
Tests that validate multiple systems working together correctly.

### Performance Tests
Tests that measure system performance under various conditions.

### User Experience Tests
Tests that validate user-facing functionality and interface behavior.

## Cross-References

All test files in this directory use the `@` symbol notation for cross-referencing:
- **Tasks**: `@Task_7`, `@Task_15.3`
- **Objects**: `@PlayerController`, `@EnemyAI`
- **Other Tests**: `@Test_7.1.2`, `@Test_Unit_PlayerController`

## Interactive Testing Workflow

1. **Test Creation**: Claude creates test file when task/subtask is completed
2. **Pre-Execution**: Verify preconditions are met
3. **Step-by-Step Execution**: Claude guides user through each test step
4. **Observation Collection**: User reports what they observe after each step
5. **Documentation**: Claude updates test file with actual results
6. **Follow-up**: Create fix tasks if test fails

## Quality Standards

- All tests must include complete cross-references
- Test steps must be specific and measurable
- Expected results must be observable and verifiable
- Failed tests must generate follow-up fix tasks
- Test documentation must be kept current with implementation

## Maintenance

- Review test files regularly for accuracy
- Update cross-references when related files change
- Archive obsolete tests for deprecated functionality
- Expand test coverage as features evolve 